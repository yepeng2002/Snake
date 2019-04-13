using MassTransit;
using Snake.Core.Events;
using Snake.Core.Mongo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake.ApiTrackService.Consumers
{
    public class AppLogCreatedEventConsumer : IConsumer<AppLogCreatedEvent>
    {
        public Task Consume(ConsumeContext<AppLogCreatedEvent> context)
        {
            return Task.Run(() =>
            {
                if (context.Message != null)
                {
                    var repository = MongoRepository<AppLogCreatedEvent>.Instance;
                    var obj = repository.Add(context.Message);
                }
                Console.WriteLine($"Recevied By AppLogCreatedEventConsumer:{context.Message.Guid}");
            });
        }
    }
}
