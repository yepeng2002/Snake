using Snake.Api.Services;
using Snake.Core.Enums;
using Snake.Core.Models;
using System;
using System.Web.Http;

namespace Snake.Api.Controllers
{
    [RoutePrefix("api/TrackLog")]
    public class TrackLogController : BaseApiController
    {
        [Route("PublishTrackLog")]
        [HttpPost]
        [EmBasicAuthorize]
        public TransData<string> PublishTrackLog(TrackLog trackLog)
        {
            var result = new TransData<string>();
            if (trackLog == null)
                result.Code = (int)ServiceResultCode.ParameterError;
            try
            {
                var trackLogService = GetService<TrackLogService>();
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

    }
}