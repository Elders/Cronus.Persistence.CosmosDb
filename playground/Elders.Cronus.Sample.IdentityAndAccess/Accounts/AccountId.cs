using Elders.Cronus.DomainModeling;
using System;
using System.Runtime.Serialization;

namespace Elders.Cronus.Sample.IdentityAndAccess.Accounts
{
    [DataContract(Name = "7aecdfe6-ccbc-4ce2-add6-f98ee1a552a0")]
    public class AccountId : GuidId
    {
        AccountId() { }

        public AccountId(Guid id) : base(id, "account") { }
    }
}
