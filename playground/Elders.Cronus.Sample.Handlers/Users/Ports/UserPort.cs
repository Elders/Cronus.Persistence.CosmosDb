using Elders.Cronus.DomainModeling;
using Elders.Cronus.Sample.Collaboration.Contracts.Users;
using Elders.Cronus.Sample.Collaboration.Contracts.Users.Commands;
using Elders.Cronus.Sample.IdentityAndAccess.Contracts.Accounts.Events;
using System;

namespace Elders.Cronus.Sample.Handlers.Users.Ports
{
    public class Userport : IPort,
        IEventHandler<AccountRegistered>
    {
        public IPublisher<ICommand> CommandPublisher { get; set; }

        public void Handle(AccountRegistered message)
        {
            UserId userId = new UserId(Guid.NewGuid());
            string email = message.Email;
            CommandPublisher.Publish(new CreateUser(userId, email));
        }
    }
}
