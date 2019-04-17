using MassTransit;
using Snake.Core.Events;
using Snake.Core.Models;
using Snake.Core.Mongo;
using Snake.Core.Util;
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
                    var repository = MongoRepository<AppLog>.Instance;
                    AppLog appLog = MapperProvider.MapTo<AppLog>(context.Message);
                    var obj = repository.Add(appLog);
                }
                Console.WriteLine($"Recevied By AppLogCreatedEventConsumer:{context.Message.Guid}");
            });
        }
    }
}
