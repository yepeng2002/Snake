using MassTransit;
using Snake.Core.Events;
using Snake.Core.Models;
using Snake.Core.Util;

namespace Snake.Api.Services
{
    /// <summary>
    /// 日志逻辑服务层
    /// </summary>
    public class LogService : BaseService
    {
        private readonly IBus _bus;

        public LogService()
        {
            _bus = GetService<IBus>();
        }

        public void CreateTrackLog(TrackLogCreatedEvent trackLogCreatedEvent)
        {
            _bus.Publish(trackLogCreatedEvent);
        }

        public void CreateAppLog(AppLogCreatedEvent appLogCreatedEvent)
        {
            _bus.Publish(appLogCreatedEvent);
        }
    }
}