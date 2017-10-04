using MassTransit;
using Snake.Api.Models;
using Snake.Core.Events;
using Snake.Core.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Snake.Api.Services
{
    public class TrackLogService : BaseService
    {
        private readonly IBus _bus;

        public TrackLogService()
        {
            _bus = GetService<IBusControl>();
        }

        public void CreateTrackLog(TrackLog trackLog)
        {
            TrackLogCreatedEvent trackLogCreatedEvent = MapperProvider.MapTo<TrackLogCreatedEvent>(trackLog);
            _bus.Publish(trackLogCreatedEvent);
        }
    }
}