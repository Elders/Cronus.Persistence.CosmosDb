using Cronus.Persistence.CosmosDb;
using Cronus.Persistence.CosmosDb.Logging;
using Elders.Cronus.EventStore;
using Elders.Cronus.Serializer;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Elders.Cronus.Persistence.CosmosDb
{
    public class CosmosEventStorePlayer : IEventStorePlayer
    {
        static readonly ILog log = LogProvider.GetLogger(typeof(CosmosEventStorePlayer));

        private readonly ISerializer serializer;

        private readonly DocumentClient client;

        private readonly Uri queryUri;

        public CosmosEventStorePlayer(DocumentClient client, Uri queryUri, ISerializer serializer)
        {
            this.client = client;
            this.serializer = serializer;
            this.queryUri = queryUri;
        }

        public IEnumerable<AggregateCommit> LoadAggregateCommits(int batchSize = 100)
        {
            int stupidityFactor = 0;
            bool hasMoreRecords = true;
            var options = new FeedOptions { MaxItemCount = batchSize };

            while (hasMoreRecords)
            {
                IDocumentQuery<CosmosDbDocument> query = client.CreateDocumentQuery<CosmosDbDocument>(queryUri, options).OrderBy(x => x.I).AsDocumentQuery();
                FeedResponse<Document> result = query.ExecuteNextAsync<Document>().Result;

                foreach (var cosmosDocument in result)
                {
                    byte[] data = ((CosmosDbDocument)((dynamic)cosmosDocument)).D;
                    using (var dataStream = new MemoryStream(data))
                    {
                        AggregateCommit commit;
                        try
                        {
                            commit = (AggregateCommit)serializer.Deserialize(dataStream);
                        }
                        catch (Exception ex)
                        {
                            string error = "Failed to deserialize an AggregateCommit. EventBase64bytes:" + Convert.ToBase64String(data);
                            log.ErrorException(error, ex);
                            continue;
                        }
                        yield return commit;
                    }
                }

                if (!query.HasMoreResults) hasMoreRecords = false;

                if (stupidityFactor > 1000)
                    throw new Exception("Stupidity in CosmosDB loading. Something is causing the process to cycle endlessly.");

                if (options.RequestContinuation == null) //https://codeopinion.com/paging-documentdb-query-results-from-net/
                    options.RequestContinuation = result.ResponseContinuation;
                stupidityFactor++;
            };
        }
    }
}
