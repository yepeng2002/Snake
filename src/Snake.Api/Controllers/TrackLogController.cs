using MongoDB.Driver;
using Snake.Api.Services;
using Snake.Core.Enums;
using Snake.Core.Events;
using Snake.Core.Models;
using Snake.Core.Mongo;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Http;

namespace Snake.Api.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [RoutePrefix("api/TrackLog")]
    public class TrackLogController : BaseApiController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="trackLog"></param>
        /// <returns></returns>
        [Route("PublishTrackLog")]
        [HttpPost]
        [SnakeBasicAuthorize]
        public TransData<string> PublishTrackLog([FromBody]TrackLog trackLog)
        {
            var result = new TransData<string>();
            if (trackLog == null)
            {
                return Response<string>(null, "Parameter is null", (int)ServiceResultCode.ParameterError);
            }

            try
            {
                var trackLogService = GetService<LogService>();
                trackLogService.CreateTrackLog(trackLog);
                result.Code = (int)ServiceResultCode.Succeeded;
            }
            catch (Exception e)
            {
                result.Data = null;
                result.Message = "服务器异常！";
                result.Code = (int)ServiceResultCode.UndefinedError;
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="appLog"></param>
        /// <returns></returns>
        [Route("PublishAppLog")]
        [HttpPost]
        [SnakeBasicAuthorize]
        public TransData<string> PublishAppLog([FromBody]AppLog appLog)
        {
            var result = new TransData<string>();
            if (appLog == null)
            {
                return Response<string>(null, "Parameter is null", (int)ServiceResultCode.ParameterError);
            }

            try
            {
                var trackLogService = GetService<LogService>();
                trackLogService.CreateAppLog(appLog);
                result.Code = (int)ServiceResultCode.Succeeded;
            }
            catch (Exception e)
            {
                result.Data = null;
                result.Message = "服务器异常！";
                result.Code = (int)ServiceResultCode.UndefinedError;
            }
            return result;
        }

        /// <summary>
        /// 查询日志
        /// </summary>
        /// <param name="appLog">带分页的实体</param>
        /// <returns></returns>
        [Route("GetAppLogs")]
        [HttpPost]
        [SnakeBasicAuthorize]
        public TransData<IList<AppLogCreatedEvent>> GetAppLogs([FromBody]PageAppLog appLog)
        {
            var result = new TransData<IList<AppLogCreatedEvent>>();
            if (appLog == null)
            {
                return Response<IList<AppLogCreatedEvent>>(null, "Parameter is null", (int)ServiceResultCode.ParameterError);
            }
            if (appLog.CTime == null)
            {
                return Response<IList<AppLogCreatedEvent>>(null, "CTime is null", (int)ServiceResultCode.ParameterError);
            }

            try
            {
                var repository = MongoRepository<AppLogCreatedEvent>.Instance;
                ParameterExpression parameter = Expression.Parameter(typeof(AppLogCreatedEvent));
                Expression condiction1 = Expression.GreaterThanOrEqual(Expression.Property(parameter, "CTime"), Expression.Constant(appLog.CTime));
                Expression condiction = condiction1;
                if (!string.IsNullOrEmpty(appLog.Application))
                {
                    Expression condiction2 = Expression.Equal(Expression.Property(parameter, "Application"), Expression.Constant(appLog.Application));
                    condiction = Expression.AndAlso(condiction, condiction2);
                }
                var lambda = Expression.Lambda<Func<AppLogCreatedEvent, bool>>(condiction, parameter);
                result.Data = repository.GetList(lambda);
                result.Code = (int)ServiceResultCode.Succeeded;
            }
            catch (Exception e)
            {
                result.Data = null;
                result.Message = "服务器异常！";
                result.Code = (int)ServiceResultCode.UndefinedError;
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="appLog"></param>
        /// <returns></returns>
        [Route("GetAppLogsPage")]
        [HttpPost]
        [SnakeBasicAuthorize]
        public TransData<IList<AppLogCreatedEvent>> GetAppLogsPage([FromBody]PageAppLog appLog)
        {
            var result = new TransData<IList<AppLogCreatedEvent>>();
            if (appLog == null)
            {
                return Response<IList<AppLogCreatedEvent>>(null, "Parameter is null", (int)ServiceResultCode.ParameterError);
            }
            if (appLog.CTime == null)
            {
                return Response<IList<AppLogCreatedEvent>>(null, "CTime is null", (int)ServiceResultCode.ParameterError);
            }

            try
            {
                var repository = MongoRepository<AppLogCreatedEvent>.Instance;
                var list = new List<FilterDefinition<AppLogCreatedEvent>>();
                list.Add(Builders<AppLogCreatedEvent>.Filter.Gt("CTime", appLog.CTime));
                if (!string.IsNullOrEmpty(appLog.Application))
                {
                    list.Add(Builders<AppLogCreatedEvent>.Filter.Eq("Application", appLog.Application));
                }
                FilterDefinition<AppLogCreatedEvent> filter = Builders<AppLogCreatedEvent>.Filter.And(list);
                var sort = Builders<AppLogCreatedEvent>.Sort.Descending("CTime");
                result.Data = repository.GetPageOrderBy(filter, sort, new QueryParams() { Index = appLog.PageIndex, Size = appLog.PageSize });
                result.Code = (int)ServiceResultCode.Succeeded;
            }
            catch (Exception e)
            {
                result.Data = null;
                result.Message = "服务器异常！";
                result.Code = (int)ServiceResultCode.UndefinedError;
            }
            return result;
        }
    }
}