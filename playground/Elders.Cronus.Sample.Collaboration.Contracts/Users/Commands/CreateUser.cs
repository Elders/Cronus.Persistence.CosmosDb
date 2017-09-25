using Elders.Cronus.DomainModeling;
using System.Runtime.Serialization;

namespace Elders.Cronus.Sample.Collaboration.Contracts.Users.Commands
{
    [DataContract(Name = "3be31640-78d5-47c8-8f0d-8f296e11d3b8")]
    public class CreateUser : ICommand
    {
        CreateUser()
        { }

        public CreateUser(UserId id, string email)
        {
            Email = email;
            Id = id;
        }

        [DataMember(Order = 1)]
        public UserId Id { get; private set; }

        [DataMember(Order = 2)]
        public string Email { get; private set; }

        public override string ToString()
        {
            return this.ToString($"Create a new user with '{Email}' email. {Id}");
        }
    }
}
