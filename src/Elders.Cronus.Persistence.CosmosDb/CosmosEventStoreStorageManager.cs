using Elders.Cronus.EventStore;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using System;

namespace Elders.Cronus.Persistence.CosmosDb
{
    public class CosmosEventStoreStorageManager : IEventStoreStorageManager
    {
        private readonly DocumentClient client;
        private readonly string databaseName;
        private readonly string collectionId;
        private readonly int throughput;
        private readonly bool withMultiplePartition;
        private readonly IndexingPolicy indexingPolicy;

        public CosmosEventStoreStorageManager(DocumentClient client, string databaseName, string collectionId, int throughput, bool withMultiplePartition, IndexingPolicy indexingPolicy)
        {
            this.client = client;
            this.databaseName = databaseName;
            this.collectionId = collectionId;
            this.throughput = throughput;
            this.withMultiplePartition = withMultiplePartition;
            this.indexingPolicy = indexingPolicy;
        }

        public void CreateEventsStorage()
        {
            var newCollection = new DocumentCollection { Id = collectionId };
            if (withMultiplePartition)
                newCollection.PartitionKey.Paths.Add("/i");

            Uri databaseUri = UriFactory.CreateDatabaseUri(databaseName);

            newCollection.IndexingPolicy = indexingPolicy;

            client.CreateDocumentCollectionIfNotExistsAsync(databaseUri, newCollection, new RequestOptions { OfferThroughput = throughput }).Wait();
        }

        public void CreateSnapshotsStorage()
        {
        }

        public void CreateStorage()
        {
            client.CreateDatabaseIfNotExistsAsync(new Database { Id = databaseName }).Wait();
            CreateEventsStorage();
        }
    }
}
