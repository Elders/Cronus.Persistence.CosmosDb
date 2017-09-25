using Elders.Cronus.AtomicAction.Config;
using Elders.Cronus.Cluster.Config;
using Elders.Cronus.DomainModeling;
using Elders.Cronus.IocContainer;
using Elders.Cronus.Pipeline.Hosts;
using Elders.Cronus.Sample.Collaboration.Contracts.Users.Events;
using Elders.Cronus.Sample.IdentityAndAccess.Contracts.Accounts.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Elders.Cronus.Sample.ApplicationServices
{
    class Program
    {
        static void Main(string[] args)
        {


        }

        static void UseCronusHostWithCosmosEventStore()
        {
            var container = new Container();
            var cfg = new CronusSettings(container)
                .UseCluster(cluster => cluster.UseAggregateRootAtomicAction(atomic => atomic.WithInMemory()));
            //.UseContractsFromAssemblies(new[] { Assembly.GetAssembly(typeof(AccountRegistered)), Assembly.GetAssembly(typeof(UserCreated)) });







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
