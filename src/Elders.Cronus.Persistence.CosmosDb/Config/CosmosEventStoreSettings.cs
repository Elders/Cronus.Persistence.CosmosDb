using Elders.Cronus.DomainModeling;
using Elders.Cronus.EventStore;
using Elders.Cronus.EventStore.Config;
using Elders.Cronus.IocContainer;
using Elders.Cronus.Pipeline.Config;
using Elders.Cronus.Serializer;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using System;

namespace Cronus.Persistence.CosmosDb.Config
{
    public static class CosmosEventStoreExtensions
    {
        public static T UseCosmosEventStore<T>(this T self, Action<CosmosEventStoreSettings> configure) where T : IConsumerSettings<ICommand>
        {
            CosmosEventStoreSettings settings = new CosmosEventStoreSettings(self);
            settings.SetDatabaseName("Elders");
            settings.SetCollectionName("EventStore");
            settings.SetThroughput(400);
            configure?.Invoke(settings);

            (settings as ISettingsBuilder).Build();
            return self;
        }

        public static T SetDatabaseName<T>(this T self, string databaseName) where T : ICosmosEventStoreSettings
        {
            if (string.IsNullOrWhiteSpace(databaseName)) throw new ArgumentNullException(nameof(databaseName));

            self.DatabaseName = databaseName;

            return self;
        }

        /// <summary>
        /// Set the Request Units per second 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"></param>
        /// <param name="throughput"></param>
        /// <returns></returns>
        public static T SetThroughput<T>(this T self, int throughput) where T : ICosmosEventStoreSettings
        {
            if (throughput < 400) throw new ArgumentException("Min is 400!", nameof(throughput));

            self.Throughput = throughput;

            return self;
        }

        public static T SetCollectionName<T>(this T self, string collectionName) where T : ICosmosEventStoreSettings
        {
            if (string.IsNullOrWhiteSpace(collectionName)) throw new ArgumentNullException(nameof(collectionName));

            self.CollectionName = collectionName;

            return self;
        }


        public static T SetDocumentclient<T>(this T self, Uri uri, string masterKey) where T : ICosmosEventStoreSettings
        {
            if (uri == null || string.IsNullOrEmpty(masterKey)) throw new ArgumentException("Supply correct information for the DocumentClient Uri and/or MasterKey.");

            self.DocumentClient = new DocumentClient(uri, masterKey);

            return self;
        }

        public static T WithNewStorageIfNotExists<T>(this T self) where T : ICosmosEventStoreSettings
        {
            self.WithNewStorageIfNotExists = true;

            return self;
        }

        /// <summary>
        /// This sets the partition key to "/i". Requires the set ThroughPut to be >=2500
        /// https://azure.microsoft.com/en-us/blog/10-things-to-know-about-documentdb-partitioned-collections/
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"></param>
        /// <returns></returns>
        public static T WithMultiplePartition<T>(this T self) where T : ICosmosEventStoreSettings
        {
            self.WithMultiplePartition = true;

            return self;
        }
    }

    public class CosmosEventStoreSettings : SettingsBuilder, ICosmosEventStoreSettings
    {
        public CosmosEventStoreSettings(ISettingsBuilder settingsBuilder) : base(settingsBuilder) { }

        public override void Build()
        {
            var builder = this as ISettingsBuilder;
            ICosmosEventStoreSettings settings = this as ICosmosEventStoreSettings;
            Uri queryUri = UriFactory.CreateDocumentCollectionUri(settings.DatabaseName, settings.CollectionName);

            if (settings.WithNewStorageIfNotExists)
            {
                settings.DocumentClient.CreateDatabaseIfNotExistsAsync(new Database { Id = settings.DatabaseName }).Wait();
                CreateAggregateCollection(settings.DocumentClient, settings.DatabaseName, settings.CollectionName, settings.Throughput, settings.WithMultiplePartition);
            }

            var eventStore = new CosmosEventStore(settings.DocumentClient, queryUri, builder.Container.Resolve<ISerializer>());

            builder.Container.RegisterSingleton<IEventStore>(() => eventStore, builder.Name);
        }

        private static void CreateAggregateCollection(DocumentClient client, string databaseId, string collectionId, int throughput, bool withMultiplePartition)
        {
            var newCollection = new DocumentCollection { Id = collectionId };
            if (withMultiplePartition)
                newCollection.PartitionKey.Paths.Add("/i");

            Uri databaseUri = UriFactory.CreateDatabaseUri(databaseId);

            client.CreateDocumentCollectionIfNotExistsAsync(databaseUri, newCollection, new RequestOptions { OfferThroughput = throughput }).Wait();
        }

        string ICosmosEventStoreSettings.DatabaseName { get; set; }

        string ICosmosEventStoreSettings.CollectionName { get; set; }

        int ICosmosEventStoreSettings.Throughput { get; set; }

        bool ICosmosEventStoreSettings.WithNewStorageIfNotExists { get; set; }

        string IEventStoreSettings.BoundedContext { get; set; }

        DocumentClient ICosmosEventStoreSettings.DocumentClient { get; set; }

        bool ICosmosEventStoreSettings.WithMultiplePartition { get; set; }
    }

    public interface ICosmosEventStoreSettings : IEventStoreSettings
    {
        string DatabaseName { get; set; }
        string CollectionName { get; set; }
        int Throughput { get; set; }
        bool WithNewStorageIfNotExists { get; set; }
        bool WithMultiplePartition { get; set; }
        DocumentClient DocumentClient { get; set; }
    }
}
