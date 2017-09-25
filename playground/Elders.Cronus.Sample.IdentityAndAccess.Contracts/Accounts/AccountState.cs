using Elders.Cronus.DomainModeling;
using Elders.Cronus.Sample.IdentityAndAccess.Accounts;
using Elders.Cronus.Sample.IdentityAndAccess.Accounts.Events;
using System.Runtime.Serialization;

namespace Elders.Cronus.Sample.IdentityAndAccess.Contracts.Accounts
{
    [DataContract(Name = "1aa61678-3fca-4ce6-948b-e965f6936b36")]
    public class AccountState : AggregateRootState<Account, AccountId>
    {
        public AccountState() { }

        [DataMember(Order = 1)]
        public override AccountId Id { get; set; }

        [DataMember(Order = 2)]
        public string Email { get; private set; }

        [DataMember(Order = 3)]
        public string Firstname { get; private set; }

        [DataMember(Order = 4)]
        public string Lastname { get; private set; }

        public void When(AccountRegistered e)
        {
            Id = e.Id;
            Email = Email;
        }

        public void When(AccountEmailChanged e)
        {
            Email = e.NewEmail;
        }
    }
}
