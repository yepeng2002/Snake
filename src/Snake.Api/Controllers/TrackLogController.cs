using Snake.Api.Enums;
using Snake.Api.Models;
using Snake.Api.Services;
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
        public TransData<string> PublishTrackLog(TrackLog model)
        {
            var result = new TransData<string>();
            if (model == null)
                result.state = (int)ServiceResultCode.ParameterError;
            try
            {
                var trackLogService = GetService<TrackLogService>();
                trackLogService.CreateTrackLog(model);
                result.state = (int)ServiceResultCode.Succeeded;
            }
            catch (Exception e)
            {
                result.Data = null;
                result.message = "服务器异常！";
                result.state = (int)ServiceResultCode.UndefinedError;
            }
            return result;
        }

    }
}