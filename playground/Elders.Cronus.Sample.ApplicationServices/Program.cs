using Cronus.Persistence.CosmosDb.Config;
using Elders.Cronus.AtomicAction.Config;
using Elders.Cronus.Cluster.Config;
using Elders.Cronus.DomainModeling;
using Elders.Cronus.IocContainer;
using Elders.Cronus.Pipeline.Config;
using Elders.Cronus.Pipeline.Hosts;
using Elders.Cronus.Pipeline.Transport.RabbitMQ.Config;
using Elders.Cronus.Sample.Collaboration.Contracts.Users.Commands;
using Elders.Cronus.Sample.Collaboration.Contracts.Users.Events;
using Elders.Cronus.Sample.IdentityAndAccess.Accounts;
using Elders.Cronus.Sample.IdentityAndAccess.Contracts.Accounts.Events;
using Elders.Cronus.Sample.Collaboration.Users;
using System;
using System.Reflection;
using Elders.Cronus.Pipeline.Transport;
using Elders.Cronus.Serializer;
using Elders.Cronus.Pipeline;
using Elders.Cronus.Sample.IdentityAndAccess.Contracts.Accounts.Commands;

namespace Elders.Cronus.Sample.ApplicationServices
{
    class Program
    {
        static CronusHost host;
        static Container container;
        static IPublisher<ICommand> commandPublisher;

        static void Main(string[] args)
        {
            container = new Container();
            UseCronusHostWithCosmosEventStore();
            var guid = Guid.NewGuid();
            commandPublisher.Publish(new CreateUser(new Collaboration.Contracts.Users.UserId(guid), "CoolEmail@gmail.com"));
            commandPublisher.Publish(new RenameUser(new Collaboration.Contracts.Users.UserId(guid), "alexios", "plomaritis"));
            commandPublisher.Publish(new RenameUser(new Collaboration.Contracts.Users.UserId(guid), "barlexios", "Blomaritis"));
        }

        static void UseCronusHostWithCosmosEventStore()
        {
            var cfg = new CronusSettings(container)
                .UseCluster(cluster => cluster.UseAggregateRootAtomicAction(atomic => atomic.WithInMemory()))
                .UseContractsFromAssemblies(new[] { Assembly.GetAssembly(typeof(AccountRegistered)), Assembly.GetAssembly(typeof(UserCreated)) });

            string IAA = "IAA";
            var IAA_appServiceFactory = new ApplicationServiceFactory(container, IAA);
            cfg.UseCommandConsumer(IAA, consumer => consumer
                   .UseRabbitMqTransport(x => x.Server = "docker-local.com")
                   .SetNumberOfConsumerThreads(1)
                   .WithDefaultPublishers()
                   .UseCosmosEventStore(eventStore => eventStore
                        .SetDocumentclient(new Uri("https://localhost:8081"), "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==")
                        .SetThroughput(2500)
                        .WithNewStorageIfNotExists())
                   .UseApplicationServices(cmdHandler => cmdHandler.RegisterHandlersInAssembly(new[] { typeof(AccountAppService).Assembly }, IAA_appServiceFactory.Create)));

            string COLL = "COLL";
            var COLL_appServiceFactory = new ApplicationServiceFactory(container, COLL);
            cfg.UseCommandConsumer(COLL, consumer => consumer
                   .UseRabbitMqTransport(x => x.Server = "docker-local.com")
                   .SetNumberOfConsumerThreads(1)
                   .WithDefaultPublishers()
                   .UseCosmosEventStore(eventStore => eventStore
                        .SetDocumentclient(new Uri("https://localhost:8081"), "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==")
                        .SetThroughput(2500)
                        .WithNewStorageIfNotExists())
                   .UseApplicationServices(cmdHandler => cmdHandler.RegisterHandlersInAssembly(new[] { typeof(UserAppService).Assembly }, COLL_appServiceFactory.Create)));

            (cfg as ISettingsBuilder).Build();
            host = container.Resolve<CronusHost>();
            host.Start();

            ConfigureRabbitMQPublisher();

        }

        private static void ConfigureRabbitMQPublisher()
        {

            var container = new Container();
            Func<IPipelineTransport> transport = () => container.Resolve<IPipelineTransport>();
            Func<ISerializer> serializer = () => container.Resolve<ISerializer>();
            container.RegisterSingleton<IPublisher<ICommand>>(() => new PipelinePublisher<ICommand>(transport(), serializer()));

            var cfg = new CronusSettings(container)
                .UseContractsFromAssemblies(new Assembly[] { Assembly.GetAssembly(typeof(RegisterAccount)), Assembly.GetAssembly(typeof(CreateUser)) })
                .UseRabbitMqTransport(x => x.Server = "docker-local.com");
            (cfg as ISettingsBuilder).Build();
            commandPublisher = container.Resolve<IPublisher<ICommand>>();
        }

        public class ApplicationServiceFactory
        {
            private readonly IContainer container;
            private readonly string namedInstance;

            public ApplicationServiceFactory(IContainer container, string namedInstance)
            {
                this.container = container;
                this.namedInstance = namedInstance;
            }

            public object Create(Type appServiceType)
            {
                var appService = FastActivator
                    .CreateInstance(appServiceType)
                    .AssignPropertySafely<IAggregateRootApplicationService>(x => x.Repository = container.Resolve<IAggregateRepository>(namedInstance));
                return appService;
            }
        }
    }
}
