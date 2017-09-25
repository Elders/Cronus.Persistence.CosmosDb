using Elders.Cronus.DomainModeling;
using System.Runtime.Serialization;

namespace Elders.Cronus.Sample.Collaboration.Contracts.Users.Commands
{
    [DataContract(Name = "a21578b3-3219-4a5e-bdb5-6e0bc8ad8ad1")]
    public class RenameUser : ICommand
    {
        RenameUser() { }

        public RenameUser(UserId id, string firstname, string lastname)
        {
            Id = id;
            Firstname = firstname;
            Lastname = lastname;
        }

        [DataMember(Order = 1)]
        public string Firstname { get; private set; }

        [DataMember(Order = 2)]
        public string Lastname { get; private set; }

        [DataMember(Order = 3)]
        public UserId Id { get; private set; }

        public override string ToString()
        {
            return this.ToString($"Rename a user with '{Firstname}' firstname and '{Lastname} lastname. '{Id}");
        }
    }
}
