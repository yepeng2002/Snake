﻿using MongoDB.Driver;
using Snake.Api.Services;
using Snake.Core.Enums;
using Snake.Core.Events;
using Snake.Core.Models;
using Snake.Core.Mongo;
using Snake.Core.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;

namespace Snake.Api.Controllers
{
    /// <summary>
    /// 日志管理控制器
    /// </summary>
    [RoutePrefix("api/TrackLog")]
    public class TrackLogController : BaseApiController
    {
        /// <summary>
        /// 提交api监控日志消息
        /// </summary>
        /// <param name="trackLogCreatedEvent">新增api监控日志事件实体</param>
        /// <returns></returns>
        [Route("PublishTrackLog")]
        [HttpPost]
        [SnakeBasicAuthorize]
        [ApiExplorerSettings(IgnoreApi = true)]
        public TransData<string> PublishTrackLog([FromBody]TrackLogCreatedEvent trackLogCreatedEvent)
        {
            var result = new TransData<string>();
            if (trackLogCreatedEvent == null)
            {
                return Response<string>(null, "Parameter is null", (int)ServiceResultCode.ParameterError);
            }

            try
            {
                var trackLogService = GetService<LogService>();
                trackLogService.CreateTrackLog(trackLogCreatedEvent);
                result.Code = (int)ServiceResultCode.Succeeded;
            }
            catch (Exception e)
            {
                result.Data = null;
                result.Message = "服务异常！";
                result.Code = (int)ServiceResultCode.UndefinedError;
            }
            return result;
        }

        /// <summary>
        /// 提交应用日志消息
        /// </summary>
        /// <param name="appLogCreatedEvent">新增应用日志事件实体</param>
        /// <returns></returns>
        [Route("PublishAppLog")]
        [HttpPost]
        [SnakeBasicAuthorize]
        public TransData<string> PublishAppLog([FromBody]AppLogCreatedEvent appLogCreatedEvent)
        {
            var result = new TransData<string>();
            if (appLogCreatedEvent == null)
            {
                return Response<string>(null, "Parameter is null", (int)ServiceResultCode.ParameterError);
            }

            try
            {
                var trackLogService = GetService<LogService>();
                trackLogService.CreateAppLog(appLogCreatedEvent);
                result.Code = (int)ServiceResultCode.Succeeded;
            }
            catch (Exception e)
            {
                result.Data = null;
                result.Message = "服务异常！";
                result.Code = (int)ServiceResultCode.UndefinedError;
            }
            return result;
        }

        /// <summary>
        /// 查询日志带分页
        /// </summary>
        /// <param name="pageAppLog">带分页的日志请求</param>
        /// <returns></returns>
        [Route("GetAppLogsPage")]
        [HttpPost]
        [SnakeBasicAuthorize]
        public TransData<IList<AppLog>> GetAppLogsPage([FromBody]PageAppLog pageAppLog)
        {
            var result = new TransData<IList<AppLog>>();
            if (pageAppLog == null)
            {
                return Response<IList<AppLog>>(null, "Parameter is null", (int)ServiceResultCode.ParameterError);
            }
            if (pageAppLog.PageIndex <= 0 || pageAppLog.PageSize <= 0)
            {
                return Response<IList<AppLog>>(null, "Page param is null", (int)ServiceResultCode.ParameterError);
            }
            if (pageAppLog.CTime == null || pageAppLog.CTimeEnd == null)
            {
                return Response<IList<AppLog>>(null, "CTime is null", (int)ServiceResultCode.ParameterError);
            }

            try
            {
                var repository = MongoRepository<AppLog>.Instance;
                var list = new List<FilterDefinition<AppLog>>();
                list.Add(Builders<AppLog>.Filter.Gt(x => x.CTime, pageAppLog.CTime));
                list.Add(Builders<AppLog>.Filter.Lt(x => x.CTime, pageAppLog.CTimeEnd));

                if (!string.IsNullOrEmpty(pageAppLog.Application))
                {
                    list.Add(Builders<AppLog>.Filter.Eq(x => x.Application, pageAppLog.Application));
                }
                if (!string.IsNullOrEmpty(pageAppLog.IPv4))
                {
                    list.Add(Builders<AppLog>.Filter.Eq(x => x.IPv4, pageAppLog.IPv4));
                }
                if (!string.IsNullOrEmpty(pageAppLog.Machine))
                {
                    list.Add(Builders<AppLog>.Filter.Eq(x => x.Machine, pageAppLog.Machine));
                }
                if (!string.IsNullOrEmpty(pageAppLog.LogCategory))
                {
                    list.Add(Builders<AppLog>.Filter.Eq(x => x.LogCategory, pageAppLog.LogCategory));
                }
                if (pageAppLog.Level >= 1)
                {
                    list.Add(Builders<AppLog>.Filter.Eq(x => x.Level, pageAppLog.Level));
                }
                if (!string.IsNullOrEmpty(pageAppLog.Message))
                {
                    //list.Add(Builders<AppLog>.Filter.Text(appLog.Message));  //大量消耗内存，慎用
                    list.Add(Builders<AppLog>.Filter.Eq("Message", pageAppLog.Message));
                }
                if (pageAppLog.Tags != null && pageAppLog.Tags.Count > 0)
                {
                    list.Add(Builders<AppLog>.Filter.All(x => x.Tags, pageAppLog.Tags));
                }
                FilterDefinition<AppLog> filter = Builders<AppLog>.Filter.And(list);
                var sort = Builders<AppLog>.Sort.Descending(x => x.CTime);
                result.Data = repository.GetPageOrderBy(filter, sort, new QueryParams() { Index = pageAppLog.PageIndex, Size = pageAppLog.PageSize });
                result.Code = (int)ServiceResultCode.Succeeded;
            }
            catch (Exception e)
            {
                result.Data = null;
                result.Message = "服务异常！";
                result.Code = (int)ServiceResultCode.UndefinedError;
            }
            return result;
        }

        /// <summary>
        /// 模糊查找Application名称序列
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [Route("QueryApplicationOfAppLog")]
        [HttpPost]
        [SnakeBasicAuthorize]
        public TransData<IList<string>> QueryApplicationOfAppLog([FromBody]QueryParamDto dto)
        {
            var result = new TransData<IList<string>>();
            result.Data = null;

            if (dto == null)
            {
                return Response<IList<string>>(null, "Parameter is null", (int)ServiceResultCode.ParameterError);
            }

            try
            {
                using (ICacheProvider cacheObj = CacheFactory.Instance.GetClient())
                {
                    var hs = cacheObj.GetAllItemsFromSet(CacheAppLogSet.Application);
                    if (hs != null)
                    {
                        result.Data = hs.Where(x => x.Contains(dto.StrParam)).ToList();
                    }
                }
                result.Code = (int)ServiceResultCode.Succeeded;
            }
            catch (Exception e)
            {
                result.Data = null;
                result.Message = "服务异常！";
                result.Code = (int)ServiceResultCode.UndefinedError;
            }
            return result;
        }

        /// <summary>
        /// 模糊查找Tag名称序列
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [Route("QueryTagsOfAppLog")]
        [HttpPost]
        [SnakeBasicAuthorize]
        public TransData<IList<string>> QueryTagsOfAppLog([FromBody]QueryParamDto dto)
        {
            var result = new TransData<IList<string>>();
            result.Data = null;

            if (dto == null)
            {
                return Response<IList<string>>(null, "Parameter is null", (int)ServiceResultCode.ParameterError);
            }

            try
            {
                using (ICacheProvider cacheObj = CacheFactory.Instance.GetClient())
                {
                    var hs = cacheObj.GetAllItemsFromSet(CacheAppLogSet.Tags);
                    if (hs != null)
                    {
                        result.Data = hs.Where(x => x.Contains(dto.StrParam)).ToList();
                    }
                }
                result.Code = (int)ServiceResultCode.Succeeded;
            }
            catch (Exception e)
            {
                result.Data = null;
                result.Message = "服务异常！";
                result.Code = (int)ServiceResultCode.UndefinedError;
            }
            return result;
        }
    }
}