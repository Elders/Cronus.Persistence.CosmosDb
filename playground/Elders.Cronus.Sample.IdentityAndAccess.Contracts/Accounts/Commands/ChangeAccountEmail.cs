using Elders.Cronus.DomainModeling;
using System.Runtime.Serialization;
using Elders.Cronus.Sample.IdentityAndAccess.Contracts.Accounts;

namespace Elders.Cronus.Sample.IdentityAndAccess.Accounts.Commands
{
    [DataContract(Name = "8fd88851-4915-4eb1-8bd9-1efbd7bb1ae0")]
    public class ChangeAccountEmail : ICommand
    {
        ChangeAccountEmail() { }

        public ChangeAccountEmail(AccountId id, string oldEmail, string newEmail)
        {
            Id = id;
            NewEmail = newEmail;
            OldEmail = oldEmail;
        }

        [DataMember(Order = 1)]
        public AccountId Id { get; private set; }

        [DataMember(Order = 2)]
        public string NewEmail { get; private set; }

        [DataMember(Order = 3)]
        public string OldEmail { get; private set; }

        public override string ToString()
        {
            return this.ToString($"Change old account email '{OldEmail}' to '{NewEmail}'.{Id}");
        }
    }
}
