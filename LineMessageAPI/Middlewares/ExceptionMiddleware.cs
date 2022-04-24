using LineMessageAPI.Enums;
using LineMessageAPI.Interfaces;
using LineMessageAPI.Models.Enums;
using LineMessageAPI.Models.RequestModels;
using LineMessageAPI.Services;
using System.Text.Json;

namespace LineMessageAPI.Middlewares
{
    public class ExceptionMiddleware: MiddlewareBase
    {
        private readonly IConfiguration _config;
        private readonly RequestDelegate _next;
        private readonly string _className = "ExceptionMiddleware";
        private readonly ILogService _log;

        public ExceptionMiddleware(RequestDelegate next, IConfiguration config, IEnumerable<ILogService> log)
        {
            _config = config;
            _next = next;
            _log = log.First(x => x.TypeName == LogEnum.LocalLog); //如果要取得其中一個實體
        }

        public async Task Invoke(HttpContext httpcontext, RequestIDService requestid, IWebHostEnvironment env)
        {
            string FunctionName = "Invoke";
            string requestContent = "";
            string Path = httpcontext.Request.Path;
            string Action = httpcontext.Request.Method;
            //httpContext.Response.ContentType = "application/json;charset=utf-8";
            try
            {
                httpcontext.Request.EnableBuffering();
                var reader = new StreamReader(httpcontext.Request.Body);
                requestContent = await reader.ReadToEndAsync();
                httpcontext.Request.Body.Seek(0, SeekOrigin.Begin);
                if (env.EnvironmentName.ToUpper() == "Development".ToUpper())
                    await _log.WriteAsync(_className, FunctionName, requestid, LogLevelEnum.Warning, $"Path：{Path} -{Action} -requestContent：{requestContent}");

                await _next(httpcontext);
            }
            catch (Exception ex)
            {
                await _log.WriteAsync(_className, FunctionName, requestid, LogLevelEnum.Warning, $"Path：{Path} -{Action} -requestContent：{requestContent} -ex：{ex}");
                //await httpcontext.Response.WriteAsync(JsonSerializer.Serialize(new RequestModelBase() { RequestID = requestid.RequestID, Details = ex.ToString() }));
                await HandleExceptionAsync(httpcontext, JsonSerializer.Serialize(new RequestModelBase() { RequestID = requestid.RequestID, Details = ex.ToString() }));
            }
        }

    }
}
