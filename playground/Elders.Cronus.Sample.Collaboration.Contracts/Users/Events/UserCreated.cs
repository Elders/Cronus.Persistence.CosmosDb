using Elders.Cronus.DomainModeling;
using System.Runtime.Serialization;

namespace Elders.Cronus.Sample.Collaboration.Contracts.Users.Events
{
    [DataContract(Name = "5f7f456e-b469-439c-91bf-df88a6686eea")]
    public class UserCreated : IEvent
    {
        UserCreated() { }

        public UserCreated(UserId id, string email)
        {
            Id = id;
            Email = email;
        }

        [DataMember(Order = 1)]
        public UserId Id { get; private set; }

        [DataMember(Order = 2)]
        public string Email { get; private set; }
    }
}
