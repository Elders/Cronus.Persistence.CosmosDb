using Elders.Cronus.DomainModeling;
using System.Runtime.Serialization;

namespace Elders.Cronus.Sample.Collaboration.Contracts.Users.Events
{
    [DataContract(Name = "efe691c3-9ebc-4748-974c-0dd3cce57783")]
    public class UserRenamed : IEvent
    {
        UserRenamed() { }

        public UserRenamed(UserId id, string firstname, string lastname)
        {
            Id = id;
            Firstname = firstname;
            Lastname = lastname;
        }

        [DataMember(Order = 1)]
        public UserId Id { get; private set; }

        [DataMember(Order = 2)]
        public string Firstname { get; private set; }

        [DataMember(Order = 3)]
        public string Lastname { get; private set; }
    }
}
