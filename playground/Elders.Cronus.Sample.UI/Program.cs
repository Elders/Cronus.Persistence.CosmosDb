using Elders.Cronus.DomainModeling;
using Elders.Cronus.IocContainer;
using Elders.Cronus.Pipeline;
using Elders.Cronus.Pipeline.Config;
using Elders.Cronus.Pipeline.Hosts;
using Elders.Cronus.Pipeline.Transport;
using Elders.Cronus.Sample.Collaboration.Contracts.Users;
using Elders.Cronus.Sample.Collaboration.Contracts.Users.Commands;
using Elders.Cronus.Sample.IdentityAndAccess.Accounts.Commands;
using Elders.Cronus.Sample.IdentityAndAccess.Contracts.Accounts;
using Elders.Cronus.Sample.IdentityAndAccess.Contracts.Accounts.Commands;
using Elders.Cronus.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Elders.Cronus.Sample.UI
{
    class Program
    {
        //static IPublisher<ICommand> commandPublisher;

        static void Main(string[] args)
        {


        }

        //private static void ConfigureRabbitMQPublisher()
        //{
        //    log4net.Config.XmlConfigurator.Configure();

        //    var container = new Container();
        //    Func<IPipelineTransport> transport = () => container.Resolve<IPipelineTransport>();
        //    Func<ISerializer> serializer = () => container.Resolve<ISerializer>();
        //    container.RegisterSingleton<IPublisher<ICommand>>(() => new PipelinePublisher<ICommand>(transport(), serializer()));

        //    //var cfg = new CronusSettings(container)
        //    //    .UseContractsFromAssemblies(new Assembly[] { Assembly.GetAssembly(typeof(RegisterAccount)), Assembly.GetAssembly(typeof(CreateUser)) })
        //    //    .UseRabbitMqTransport(x => x.Server = "10.0.2.4");
        //    //(cfg as ISettingsBuilder).Build();
        //    //commandPublisher = container.Resolve<IPublisher<ICommand>>();
        //}

        //private static void ConfigureAzureServuceBusPublisher()
        //{
        //    log4net.Config.XmlConfigurator.Configure();

        //    var container = new Container();
        //    Func<IPipelineTransport> transport = () => container.Resolve<IPipelineTransport>();
        //    Func<ISerializer> serializer = () => container.Resolve<ISerializer>();
        //    container.RegisterSingleton<IPublisher<ICommand>>(() => new PipelinePublisher<ICommand>(transport(), serializer()));

        //    //var cfg = new CronusSettings(container)
        //    //    .UseContractsFromAssemblies(new Assembly[] { Assembly.GetAssembly(typeof(RegisterAccount)), Assembly.GetAssembly(typeof(CreateUser)) })
        //    //    .UseAzureServiceBusTransport(x => x.ConnectionString = "Endpoint=sb://mvclientshared-servicebus-test.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=YStt1qtFInb3kp2oIj76c6ibEzlSH4oPOSjAXBkY74g=");
        //    //(cfg as ISettingsBuilder).Build();
        //    //commandPublisher = container.Resolve<IPublisher<ICommand>>();
        //}

        //private static void SingleCreationCommandFromUpstreamBC(int index)
        //{
        //    AccountId accountId = new AccountId(Guid.NewGuid());
        //    var email = String.Format("cronus_{0}_{1}_@Elders.com", index, DateTime.Now);
        //    commandPublisher.Publish(new RegisterAccount(accountId, email));
        //}

        //private static void SingleCreationCommandFromDownstreamBC(int index)
        //{
        //    UserId userId = new UserId(Guid.NewGuid());
        //    var email = String.Format("cronus_{0}_@Elders.com", index);
        //    commandPublisher.Publish(new CreateUser(userId, email));
        //}

        //private static void SingleCreateWithMultipleUpdateCommands(int index)
        //{
        //    AccountId accountId = new AccountId(Guid.NewGuid());
        //    var email = String.Format("cronus_{0}_@Elders.com", index);
        //    commandPublisher.Publish(new RegisterAccount(accountId, email));
        //    commandPublisher.Publish(new ChangeAccountEmail(accountId, email, String.Format("cronus_{0}_{0}_@Elders.com", index)));
        //    commandPublisher.Publish(new ChangeAccountEmail(accountId, email, String.Format("cronus_{0}_{0}_{0}_@Elders.com", index)));
        //    commandPublisher.Publish(new ChangeAccountEmail(accountId, email, String.Format("cronus_{0}_{0}_{0}_{0}_@Elders.com", index)));
        //    commandPublisher.Publish(new ChangeAccountEmail(accountId, email, String.Format("cronus_{0}_{0}_{0}_{0}_{0}_@Elders.com", index)));
        //    commandPublisher.Publish(new ChangeAccountEmail(accountId, email, String.Format("cronus_{0}_{0}_{0}_{0}_{0}_{0}_@Elders.com", index)));
        //}

    }
}
