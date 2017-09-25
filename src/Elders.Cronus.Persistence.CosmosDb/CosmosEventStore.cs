using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Elders.Cronus.EventStore;
using Elders.Cronus.DomainModeling;
using System;
using System.Collections.Generic;
using System.Linq;
using Elders.Cronus.Serializer;
using System.IO;

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
            string id = Convert.ToBase64String(aggregateId.RawId);
            List<CosmosDbDocument> queryResult = client.CreateDocumentQuery<CosmosDbDocument>(queryUri).Where(x => x.I == id).ToList();
            List<AggregateCommit> aggregateCommits = new List<AggregateCommit>();
            foreach (var cosmosDocument in queryResult)
            {
                var data = cosmosDocument.D;
                using (var dataStream = new MemoryStream(data))
                {
                    aggregateCommits.Add((AggregateCommit)serializer.Deserialize(dataStream));
                }
            }
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

