using System.Net;

namespace LineMessageAPI.Middlewares
{
    public class MiddlewareBase
    {
        protected async Task HandleExceptionAsync(HttpContext context, string Message)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            await context.Response.WriteAsync(Message);
        }
    }
}
