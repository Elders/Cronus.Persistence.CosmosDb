using Elders.Cronus.DomainModeling;
using System.Runtime.Serialization;

namespace Elders.Cronus.Sample.IdentityAndAccess.Contracts.Accounts.Commands
{
    [DataContract(Name = "7ffbfeca-4bb5-4e52-8dfe-81fff4b0c5a4")]
    public class RegisterAccount : ICommand
    {
        RegisterAccount() { }

        public RegisterAccount(AccountId id, string email)
        {
            Id = id;
            Email = email;
        }

        [DataMember(Order = 1)]
        public AccountId Id { get; private set; }

        [DataMember(Order = 2)]
        public string Email { get; private set; }
    }
}
