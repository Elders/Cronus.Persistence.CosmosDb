using Elders.Cronus.DomainModeling;
using System.Runtime.Serialization;

namespace Elders.Cronus.Sample.IdentityAndAccess.Accounts.Events
{
    [DataContract(Name = "8b6304b7-e28b-4678-aa78-87d080d05adb")]
    public class AccountEmailChanged : IEvent
    {
        AccountEmailChanged() { }

        public AccountEmailChanged(AccountId id, string oldEmail, string newEmail)
        {
            Id = id;
            OldEmail = oldEmail;
            NewEmail = newEmail;
        }

        [DataMember(Order = 1)]
        public AccountId Id { get; private set; }

        [DataMember(Order = 2)]
        public string OldEmail { get; private set; }

        [DataMember(Order = 3)]
        public string NewEmail { get; private set; }

        public override string ToString()
        {
            return this.ToString($"Account email '{OldEmail}' was changed to '{NewEmail}'. {Id}");
        }
    }
}
