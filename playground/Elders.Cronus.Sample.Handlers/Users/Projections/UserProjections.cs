using Elders.Cronus.DomainModeling;
using Elders.Cronus.DomainModeling.Projections;
using Elders.Cronus.Sample.Collaboration.Contracts.Users;
using Elders.Cronus.Sample.Collaboration.Contracts.Users.Events;
using System.Runtime.Serialization;

namespace Elders.Cronus.Sample.Handlers.Users.Projections
{
    [DataContract(Name = "23414adb-65e9-42f6-9e31-8e33b2ae69b1")]
    public class UserProjections : ProjectionDefinition<UserItem, UserId>, IEventHandler<UserCreated>
    {
        public UserProjections()
        {
            Subscribe<UserCreated>(x => x.Id);
        }

        public void Handle(UserCreated message)
        {

        }
    }

    [DataContract(Name = "8a96be31-1911-4cb9-a7f3-f94a3a983f70")]
    public class UserItem
    {
        [DataMember(Order = 1)]
        public UserId Id { get; set; }
    }
}
