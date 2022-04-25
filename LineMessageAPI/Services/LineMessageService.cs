using LineMessageAPI.Models.LineMessage;
using LineMessageAPI.Models.PostModels;
using Newtonsoft.Json.Linq;
using System.Text.Json;
using static LineMessageAPI.Services.APIHelper;

namespace LineMessageAPI.Services
{
    public class LineMessageService : ILineMessageService
    {
        private readonly ILogService _LocalLog;
        private readonly string? _className;
        private readonly APIHelper _api;
        private readonly LineMessageOption _lineapiseting;
        public LineMessageService(LineMessageOption lineapiseting, IEnumerable<ILogService> logservices, APIHelper api)
        {
            _LocalLog = logservices.First(x => x.TypeName == LogEnum.LocalLog);
            _className = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName;
            _api = api;
            _lineapiseting = lineapiseting; 
        }
        public async Task<string> SaveData(LineMessagePost? postdata, RequestIDService requestIDService)
        {
            string _functionname = "SaveData";
            string Message = string.Empty;
            if (postdata != null)
            {
               //JObject Jdata = JObject.FromObject(postdata);
                //if (Jdata != null)
                    //Message = Jdata.ToString();
                    Message = JsonSerializer.Serialize(postdata);
            }

            await _LocalLog.WriteAsync(_className, _functionname, requestIDService, LogLevelEnum.Info, Message);
            

            return Message;
        }

        public async Task Push(RequestIDService requestIDService, PushData data)
        {
            Uri baseUri = new Uri(_lineapiseting.APIUrl);
            Uri url = new Uri(baseUri, $"{_lineapiseting.Version}/{_lineapiseting.PushUri}");

            List<PostHeader> headers = new List<PostHeader> { 
                new PostHeader{ HeaderKey="Authorization",HeaderContent=_lineapiseting.ChannelAccessToken}
            };

            string prama = JsonSerializer.Serialize(data);

            HttpResponseMessage apiresult = await _api.Post_Async(requestIDService,url.ToString(),ContentTypeEnum.json, prama, headers);

        }

    }
}
