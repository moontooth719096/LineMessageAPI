using LineMessageAPI.Enums;
using LineMessageAPI.Interfaces;
using LineMessageAPI.Models.Enums;
using LineMessageAPI.Models.RequestModels;
using LineMessageAPI.Services;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;
using System.Net;
using System.Text.Json;

namespace LineMessageAPI.Filters
{
    public class LineMessageHeaderValidation : Attribute, IAsyncActionFilter
    {
        private readonly string _className  = "LineMessageHeaderValidation";
        private readonly ILogService _LocalLog;
        private readonly RequestIDService _requestIDService;
        public LineMessageHeaderValidation(IEnumerable<ILogService> LocalLog, RequestIDService requestIDService)
        {
            _LocalLog = LocalLog.First(x => x.TypeName == LogEnum.LocalLog);
            _requestIDService = requestIDService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            string _functionname = "OnActionExecutionAsync";
            try
            {
                if (!context.HttpContext.Request.Headers.Any(x => x.Key.ToUpper() == "X-Line-Signature".ToUpper()))
                {
                    await _LocalLog.WriteAsync(_className, _functionname, _requestIDService, LogLevelEnum.Error, "此請求沒有X-Line-Signature");
                    await HandleExceptionAsync(context.HttpContext, JsonSerializer.Serialize(new RequestModelBase() { RequestID = _requestIDService.RequestID, Details = "此請求沒有X-Line-Signature" }));
                }
                    
                else
                {
                    string LineRequestID = context.HttpContext.Request.Headers.FirstOrDefault(x => x.Key.ToUpper() == "X-Line-Signature".ToUpper()).Value;
                    await _LocalLog.WriteAsync(_className, _functionname, _requestIDService, LogLevelEnum.Info,$"x-line-request-id：{LineRequestID}");
                    await next();
                }
            }
            catch (Exception ex)
            {
                await _LocalLog.WriteAsync(_className, _functionname, _requestIDService, LogLevelEnum.Warning, ex.Message);
                await HandleExceptionAsync(context.HttpContext, JsonSerializer.Serialize(new RequestModelBase() { RequestID = _requestIDService.RequestID, Details = ex.ToString() }));
            }
        }
        private async Task HandleExceptionAsync(HttpContext context,string Message)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            await context.Response.WriteAsync(Message);
        }
    }
}
