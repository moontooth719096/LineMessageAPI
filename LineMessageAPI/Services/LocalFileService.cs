using LineMessageAPI.Enums;
using LineMessageAPI.Models.Enums;

namespace LineMessageAPI.Services
{
    public class LocalFileService : ILogService
    {
        public LogEnum TypeName => LogEnum.LocalLog;
        private string RootFileName = "LocalFile";

        public async Task<string> WriteAsync(string? ClassName, string FunctionName, RequestIDService _requestIDService, LogLevelEnum level, string Message)
        {
            string FilePath = "Log";
            string FileName = DateTime.Today.ToString("yyyyMMdd");
            string CompletePath = Path.Combine(RootFileName, FilePath);
            DateTime Nowtime = DateTime.Now;
            string WriteMessage = $"{Nowtime.ToString("yyyy-MM-dd HH:mm:ss")}|{_requestIDService.RequestID}|{_requestIDService.ClientIP}|{level.ToString()}|{Message}";
            if (CheckFile(CompletePath))
            {
                try
                {
                    //FileMode.Create 覆蓋檔案
                    using (FileStream file = new FileStream(Path.Combine(CompletePath, $"{ FileName }.txt"),FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
                    using (StreamWriter sw = new StreamWriter(file))
                    {
                        await sw.WriteLineAsync(WriteMessage);
                    }
                }
                catch(Exception ex)
                { 
                
                }
            }

            return CompletePath;
        }

        private bool CheckFile(string CompletePath)
        {
            try
            {
                if (!Directory.Exists(CompletePath))
                    Directory.CreateDirectory(CompletePath);

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
