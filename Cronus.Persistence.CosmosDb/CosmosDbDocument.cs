using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace cosmos_db_docdb_dotnet_tutorial
{
    [DataContract(Name = "6a63389b-b58f-4164-9fe9-9ae75da30067")]
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

        [DataMember(Order = 2)]
        public byte[] D { get; private set; }
    }
}
