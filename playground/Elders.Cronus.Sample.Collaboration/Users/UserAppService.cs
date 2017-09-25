using Elders.Cronus.DomainModeling;
using Elders.Cronus.Sample.Collaboration.Contracts.Users.Commands;

namespace Elders.Cronus.Sample.Collaboration.Users
{
    class UserAppService : AggregateRootApplicationService<User>,
        ICommandHandler<CreateUser>,
        ICommandHandler<RenameUser>
    {
        public void Handle(RenameUser command)
        {
            Update(command.Id, user => user.Rename(command.Firstname, command.Firstname));
        }

        public void Handle(CreateUser command)
        {
            Repository.Save(new User(command.Id, command.Email));
        }
    }
}
