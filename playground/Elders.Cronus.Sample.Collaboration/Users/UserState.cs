using Elders.Cronus.DomainModeling;
using Elders.Cronus.Sample.Collaboration.Contracts.Users;
using Elders.Cronus.Sample.Collaboration.Contracts.Users.Events;
using System.Runtime.Serialization;

namespace Elders.Cronus.Sample.Collaboration.Users
{
    [DataContract(Name = "cb49e049-380a-4361-a1aa-dcbff9959b40")]
    class UserState : AggregateRootState<User, UserId>
    {
        public UserState() { }

        [DataMember(Order = 1)]
        public override UserId Id { get; set; }

        [DataMember(Order = 2)]
        public string Email { get; private set; }

        [DataMember(Order = 3)]
        public string Firstname { get; private set; }

        [DataMember(Order = 4)]
        public string Lastname { get; private set; }

        public void When(UserRenamed e)
        {
            Firstname = e.Firstname;
            Lastname = e.Lastname;
        }

        public void When(UserCreated e)
        {
            Id = e.Id;
            Email = e.Email;
        }
    }
}
