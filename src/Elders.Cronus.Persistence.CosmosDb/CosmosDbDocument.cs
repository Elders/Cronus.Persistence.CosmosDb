using Elders.Cronus.EventStore;
using Newtonsoft.Json;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace Cronus.Persistence.CosmosDb
{
    [DataContract(Name = "00a7e299-e667-4a53-9728-eaed26765bce")]
    public class CosmosDbDocument
    {
        CosmosDbDocument() { }

        public CosmosDbDocument(string aggregateRootId, byte[] data)
        {
            I = aggregateRootId;
            D = data;
        }

        [DataMember(Order = 1), JsonProperty("i")]
        public string I { get; private set; }

        [DataMember(Order = 2), JsonProperty("d")]
        public byte[] D { get; private set; }
    }
}
