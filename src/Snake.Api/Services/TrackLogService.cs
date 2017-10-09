using MassTransit;
using Snake.Core.Events;
using Snake.Core.Models;
using Snake.Core.Util;

namespace Snake.Api.Services
{
    public class TrackLogService : BaseService
    {
        private readonly IBus _bus;

        public TrackLogService()
        {
            _bus = GetService<IBus>();
        }

        public void CreateTrackLog(TrackLog trackLog)
        {
            TrackLogCreatedEvent trackLogCreatedEvent = MapperProvider.MapTo<TrackLogCreatedEvent>(trackLog);
            _bus.Publish(trackLogCreatedEvent);
        }
    }
}