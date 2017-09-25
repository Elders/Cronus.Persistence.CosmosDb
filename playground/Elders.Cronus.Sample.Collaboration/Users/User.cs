using Elders.Cronus.DomainModeling;
using Elders.Cronus.Sample.Collaboration.Contracts.Users;
using Elders.Cronus.Sample.Collaboration.Contracts.Users.Events;
using System;

namespace Elders.Cronus.Sample.Collaboration.Users
{
    class User : AggregateRoot<UserState>
    {
        User() { }

        public User(UserId collaboratorId, string email)
        {
            if (string.IsNullOrEmpty(email)) throw new ArgumentException(nameof(email));

            var evnt = new UserCreated(collaboratorId, email);
            Apply(evnt);
        }

        public void Rename(string firstName, string lastName)
        {
            if (string.IsNullOrEmpty(firstName)) throw new ArgumentException(nameof(firstName));
            if (string.IsNullOrEmpty(lastName)) throw new ArgumentException(nameof(lastName));

            var evnt = new UserRenamed(state.Id, firstName, lastName);
            Apply(evnt);
        }
    }
}
