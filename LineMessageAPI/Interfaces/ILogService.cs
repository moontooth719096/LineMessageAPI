
using LineMessageAPI.Enums;
using LineMessageAPI.Models.Enums;
using LineMessageAPI.Services;

namespace LineMessageAPI.Interfaces
{
    public interface ILogService
    {
        public LogEnum TypeName { get; }
        Task<string> WriteAsync(string? ClassName,string FunctionName, RequestIDService _requestIDService, LogLevelEnum level, string Message);
    }
}