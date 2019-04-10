using MassTransit;
using Snake.Core.Events;
using Snake.Core.Models;
using Snake.Core.Util;

namespace Snake.Api.Services
{
    public class LogService : BaseService
    {
        private readonly IBus _bus;

        public LogService()
        {
            _bus = GetService<IBus>();
        }

        public void CreateTrackLog(TrackLog trackLog)
        {
            TrackLogCreatedEvent trackLogCreatedEvent = MapperProvider.MapTo<TrackLogCreatedEvent>(trackLog);
            _bus.Publish(trackLogCreatedEvent);
        }

        public void CreateAppLog(AppLog appLog)
        {
            AppLogCreatedEvent appLogCreatedEvent = MapperProvider.MapTo<AppLogCreatedEvent>(appLog);
            _bus.Publish(appLogCreatedEvent);
        }
    }
}