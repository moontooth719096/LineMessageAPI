using LineMessageAPI.Enums;
using LineMessageAPI.Filters;
using LineMessageAPI.Interfaces;
using LineMessageAPI.Models.Enums;
using LineMessageAPI.Models.PostModels;
using LineMessageAPI.Models.RequestModels;
using LineMessageAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace LineMessageAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LineMessageController : Controller
    {
        private readonly ILineMessageService _lineMessageService;
        private readonly RequestIDService _requestIDService;
        private readonly ILogService _LocalLog;
        private readonly string _className = "_className";

        public LineMessageController(ILineMessageService lineMessageService, RequestIDService requestIDService, IEnumerable<ILogService> logservices)
        {
            _lineMessageService = lineMessageService;
            _requestIDService = requestIDService;
            _LocalLog = logservices.First(x => x.TypeName == LogEnum.LocalLog);
        }
        [HttpPost]
        [TypeFilter(typeof(LineMessageHeaderValidation))]
        public async Task<IActionResult> Index()//LineMessagePost data
        {
            string _functionname = "Index";

            LineMessagePost? postData =await Request.HttpContext.Request.ReadFromJsonAsync<LineMessagePost>(); 
            //await _LocalLog.WriteAsync(_className, _functionname, _requestIDService, LogLevelEnum.Info, JObject.FromObject(postData).ToString());
            
            
           string data =  await _lineMessageService.SaveData(postData, _requestIDService);
            //JObject q  = new JObject();
            RequestModelBase aa = new RequestModelBase();
            return StatusCode((int)aa.Statuscode, aa);
        }
    }
}
