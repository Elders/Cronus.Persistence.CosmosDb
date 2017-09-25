using Elders.Cronus.DomainModeling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Elders.Cronus.Sample.Collaboration.Contracts.Users
{
    [DataContract(Name = "d052849c-4083-43a9-87b8-27196bcb4889")]
    public class UserId : GuidId
    {
        public UserId() { }

        public UserId(Guid id) : base(id, "user") { }
    }
}
