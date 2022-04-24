using LineMessageAPI.Enums;
using LineMessageAPI.Interfaces;
using LineMessageAPI.Models.Enums;
using LineMessageAPI.Models.PostModels;
using Newtonsoft.Json.Linq;

namespace LineMessageAPI.Services
{
    public class LineMessageService : ILineMessageService
    {
        private readonly ILogService _LocalLog;
        private readonly string _className;
        public LineMessageService(IEnumerable<ILogService> logservices)
        {
            _LocalLog = logservices.SingleOrDefault(x => x.TypeName == LogEnum.LocalLog);
            _className = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName;

        }
        public async Task SaveData(LineMessagePost postdata, RequestIDService requestIDService)
        {
            string _functionname = "SaveData";
            string Message = string.Empty;
            if (postdata != null)
            {
                JObject Jdata = JObject.FromObject(postdata);
                if (Jdata != null)
                    Message = Jdata.ToString();
            }

            await _LocalLog.WriteAsync(_className, _functionname, requestIDService, LogLevelEnum.Info, Message);
        }
    }
}
