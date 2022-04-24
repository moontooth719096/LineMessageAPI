using LineMessageAPI.Enums;
using LineMessageAPI.Filters;
using LineMessageAPI.Interfaces;
using LineMessageAPI.Models.Enums;
using LineMessageAPI.Models.PostModels;
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
            _LocalLog = logservices.SingleOrDefault(x => x.TypeName == LogEnum.LocalLog);
        }
        [HttpPost]
        [TypeFilter(typeof(LineMessageHeaderValidation))]
        public async Task<IActionResult> Index([FromBody] LineMessagePost data)//LineMessagePost data
        {
            string _functionname = "Index";

            //var postData = Request.HttpContext.Request.ReadFromJsonAsync<LineMessagePost>().Result; 
            //await _LocalLog.WriteAsync(_className, _functionname, _requestIDService, LogLevelEnum.Info, JObject.FromObject(postData).ToString());
            
            
            await _lineMessageService.SaveData(data, _requestIDService);
            //JObject q  = new JObject();   
            return Ok("aa");
        }
    }
}
