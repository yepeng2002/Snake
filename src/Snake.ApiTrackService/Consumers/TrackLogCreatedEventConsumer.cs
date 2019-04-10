using MassTransit;
using Snake.Core.Events;
using Snake.Core.Mongo;
using System;
using System.Threading.Tasks;

namespace Snake.ApiTrackService.Consumers
{
    public class TrackLogCreatedEventConsumer : IConsumer<TrackLogCreatedEvent>
    {
        public Task Consume(ConsumeContext<TrackLogCreatedEvent> context)
        {
            return Task.Run(() =>
            {
                if (context.Message != null)
                {
                    var repository = MongoRepository<TrackLogCreatedEvent>.Instance;
                    var obj = repository.Add(context.Message);
                }
                Console.WriteLine($"Recevied By Consumer:{context.Message.GUID}");
            });
        }
    }
}
