using MassTransit;
using Snake.Core.Enums;
using Snake.Core.Events;
using Snake.Core.Models;
using Snake.Core.Mongo;
using Snake.Core.Redis;
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
                    try
                    {
                        var repository = MongoRepository<AppLog>.Instance;
                        AppLog appLog = MapperProvider.MapTo<AppLog>(context.Message);
                        appLog.CTime = appLog.CTime.AddHours(8); // UTC时间转换
                        var obj = repository.Add(appLog);
                        //保存应用名和标签到缓存集合
                        using (ICacheProvider cacheObj = CacheFactory.Instance.GetClient())
                        {
                            if (!string.IsNullOrEmpty(appLog.Application))
                                cacheObj.AddItemToSet(CacheAppLogSet.Application, appLog.Application);
                            if (appLog.Tags != null)
                                cacheObj.AddRangeToSet(CacheAppLogSet.Tags, appLog.Tags);
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
                Console.WriteLine($"Recevied By AppLogCreatedEventConsumer:{context.Message.Guid}");
            });
        }
    }
}
