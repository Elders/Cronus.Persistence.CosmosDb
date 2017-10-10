﻿using Elders.Cronus.DomainModeling;
using Elders.Cronus.EventStore;
using Elders.Cronus.IocContainer;
using Elders.Cronus.Persistence.CosmosDb;
using Elders.Cronus.Pipeline;
using Elders.Cronus.Pipeline.Config;
using Elders.Cronus.Pipeline.Hosts;
using Elders.Cronus.Pipeline.Transport;
using Elders.Cronus.Pipeline.Transport.RabbitMQ.Config;
using Elders.Cronus.Sample.Collaboration.Contracts.Users;
using Elders.Cronus.Sample.Collaboration.Contracts.Users.Commands;
using Elders.Cronus.Sample.IdentityAndAccess.Accounts.Commands;
using Elders.Cronus.Sample.IdentityAndAccess.Contracts.Accounts;
using Elders.Cronus.Sample.IdentityAndAccess.Contracts.Accounts.Commands;
using Elders.Cronus.Serializer;
using Microsoft.Azure.Documents.Client;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;

namespace Elders.Cronus.Sample.UI
{
    class Program
    {
        static IPublisher<ICommand> commandPublisher;

        static void Main(string[] args)
        {
            //ConfigureRabbitMQPublisher();

            //HostUI(/////////////////////////////////////////////////////////////////
            //                   publish: SingleCreateWithMultipleUpdateCommands,
            //       delayBetweenBatches: 100,
            //                 batchSize: 100,
            //    numberOfMessagesToSend: 1000
            //    ///////////////////////////////////////////////////////////////////
            //    );
            TestCosmosDbPlayer();
        }

        private static void ConfigureRabbitMQPublisher()
        {
            log4net.Config.XmlConfigurator.Configure();

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

        private static void SingleCreationCommandFromUpstreamBC(int index)
        {
            AccountId accountId = new AccountId(Guid.NewGuid());
            var email = String.Format("cronus_{0}_{1}_@Elders.com", index, DateTime.Now);
            commandPublisher.Publish(new RegisterAccount(accountId, email));
        }

        private static void SingleCreationCommandFromDownstreamBC(int index)
        {
            UserId userId = new UserId(Guid.NewGuid());
            var email = String.Format("cronus_{0}_@Elders.com", index);
            commandPublisher.Publish(new CreateUser(userId, email));
        }

        private static void SingleCreateWithMultipleUpdateCommands(int index)
        {
            AccountId accountId = new AccountId(Guid.NewGuid());
            var email = String.Format("cronus_{0}_@Elders.com", index);
            commandPublisher.Publish(new RegisterAccount(accountId, email));
            commandPublisher.Publish(new ChangeAccountEmail(accountId, email, String.Format("cronus_{0}_{0}_@Elders.com", index)));
            commandPublisher.Publish(new ChangeAccountEmail(accountId, email, String.Format("cronus_{0}_{0}_{0}_@Elders.com", index)));
            commandPublisher.Publish(new ChangeAccountEmail(accountId, email, String.Format("cronus_{0}_{0}_{0}_{0}_@Elders.com", index)));
            commandPublisher.Publish(new ChangeAccountEmail(accountId, email, String.Format("cronus_{0}_{0}_{0}_{0}_{0}_@Elders.com", index)));
            commandPublisher.Publish(new ChangeAccountEmail(accountId, email, String.Format("cronus_{0}_{0}_{0}_{0}_{0}_{0}_@Elders.com", index)));
        }

        private static void TestCosmosDbPlayer()
        {
            var uri = new Uri("https://localhost:8081");
            var documentClient = new DocumentClient(uri, "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==");
            var queryUri = UriFactory.CreateDocumentCollectionUri("Elders", "EventStore");
            Assembly[] contracts = { typeof(RegisterAccount).Assembly, typeof(AggregateCommit).Assembly };
            var serializer = new Serialization.NewtonsoftJson.JsonSerializer(contracts);

            var cosmosPlayer = new CosmosEventStorePlayer(documentClient, queryUri, serializer);
            List<AggregateCommit> collection = new List<AggregateCommit>();

            foreach (var item in cosmosPlayer.LoadAggregateCommits(100))
            {
                collection.Add(item);
                Console.WriteLine("Collections should be working");
            }

            Console.ReadKey();
        }

        private static void HostUI(Action<int> publish, int delayBetweenBatches = 0, int batchSize = 1, int numberOfMessagesToSend = Int32.MaxValue)
        {
            Console.WriteLine("Start sending commands...");
            if (batchSize == 1)
            {
                if (delayBetweenBatches == 0)
                {
                    for (int i = 0; i < numberOfMessagesToSend; i++)
                    {
                        publish(i);
                    }
                }
                else
                {
                    for (int i = 0; i < numberOfMessagesToSend; i++)
                    {
                        publish(i);
                        Thread.Sleep(delayBetweenBatches);
                    }
                }
            }
            else
            {
                if (delayBetweenBatches == 0)
                {
                    for (int i = 0; i <= numberOfMessagesToSend - batchSize; i = i + batchSize)
                    {
                        for (int j = 0; j < batchSize; j++)
                        {
                            publish(i + j);
                        }
                    }
                }
                else
                {
                    for (int i = 0; i <= numberOfMessagesToSend - batchSize; i = i + batchSize)
                    {
                        for (int j = 0; j < batchSize; j++)
                        {
                            publish(i + j);
                        }
                        Thread.Sleep(delayBetweenBatches);
                    }
                }
            }
        }
    }
}
