using Elders.Cronus.DomainModeling;
using System.Runtime.Serialization;

namespace Elders.Cronus.Sample.IdentityAndAccess.Contracts.Accounts.Events
{
    [DataContract(Name = "1b9296b8-254d-4eee-9da8-a33f0c280c7e")]
    public class AccountRegistered : IEvent
    {
        AccountRegistered() { }

        public AccountRegistered(AccountId id, string email)
        {
            Id = id;
            Email = email;
        }

        [DataMember(Order = 1)]
        public AccountId Id { get; private set; }

        [DataMember(Order = 2)]
        public string Email { get; private set; }

        public override string ToString()
        {
            return this.ToString($"New user registered with email '{Email}'. {Id}");
        }
    }
}
