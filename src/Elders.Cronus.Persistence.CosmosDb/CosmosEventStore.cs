using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Elders.Cronus.EventStore;
using Elders.Cronus.DomainModeling;
using System;
using System.Collections.Generic;
using System.Linq;
using Elders.Cronus.Serializer;
using System.IO;
using Microsoft.Azure.Documents.Linq;
using System.Threading.Tasks;

namespace Cronus.Persistence.CosmosDb
{
    public class CosmosEventStore : IEventStore
    {
        private readonly ISerializer serializer;

        private readonly DocumentClient client;

        private readonly Uri queryUri;

        public CosmosEventStore(DocumentClient client, Uri queryUri, ISerializer serializer)
        {
            this.client = client;
            this.serializer = serializer;
            this.queryUri = queryUri;
        }

        public EventStream Load(IAggregateRootId aggregateId)
        {
            List<AggregateCommit> aggregateCommits = new List<AggregateCommit>();
            int stupidityFactor = 0;
            bool hasMoreRecords = true;
            string id = Convert.ToBase64String(aggregateId.RawId);
            var options = new FeedOptions { MaxItemCount = 100 };

            while (hasMoreRecords)
            {
                IDocumentQuery<CosmosDbDocument> query = client.CreateDocumentQuery<CosmosDbDocument>(queryUri, options).Where(x => x.I == id).AsDocumentQuery();
                FeedResponse<Document> result = query.ExecuteNextAsync<Document>().Result;

                foreach (var cosmosDocument in result)
                {
                    byte[] data = ((CosmosDbDocument)((dynamic)cosmosDocument)).D;
                    using (var dataStream = new MemoryStream(data))
                    {
                        aggregateCommits.Add((AggregateCommit)serializer.Deserialize(dataStream));
                    }
                }

                if (!query.HasMoreResults) hasMoreRecords = false;

                if (stupidityFactor > 1000)
                    throw new Exception("Stupidity in CosmosDB loading. Description");

                if (options.RequestContinuation == null) //https://codeopinion.com/paging-documentdb-query-results-from-net/
                    options.RequestContinuation = result.ResponseContinuation;
                stupidityFactor++;
            };

            return new EventStream(aggregateCommits);
        }

        public void Append(AggregateCommit aggregateCommit)
        {
            byte[] data = SerializeEvent(aggregateCommit);
            string aggregateId = Convert.ToBase64String(aggregateCommit.AggregateRootId);
            var document = new CosmosDbDocument(aggregateId, data);

            ResourceResponse<Document> response = client.CreateDocumentAsync(queryUri, document).Result;
        }

        private byte[] SerializeEvent(AggregateCommit commit)
        {
            using (var stream = new MemoryStream())
            {
                serializer.Serialize(stream, commit);
                return stream.ToArray();
            }
        }
    }
}

