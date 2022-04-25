using System.Net.Http.Headers;
using System.Text;

namespace LineMessageAPI.Services
{
    public class APIHelper
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly ILogService _LocalLog;
        private readonly string _className = "APIHelper";
        public APIHelper(IHttpClientFactory clientFactory, IEnumerable<ILogService> logservices)
        {
            this._clientFactory = clientFactory;
            _LocalLog = logservices.First(x => x.TypeName == LogEnum.LocalLog);
        }
        public enum ContentTypeEnum
        {
            urlencoded = 1,
            json = 2
        }
        public class PostHeader
        {
            public string HeaderKey { get; set; }
            public string HeaderContent { get; set; }
        }


        public async Task<HttpResponseMessage> Post_Async(RequestIDService requestIDService,string Url, ContentTypeEnum ContentType, string Pramater = null, List<PostHeader> HeaderList = null, int Timeout = 90)
        {
            string _functionname = "Post_Async";
            HttpResponseMessage response = null;
            try
            {
                var _httpClient = this._clientFactory.CreateClient();

                if (HeaderList != null && HeaderList.Count > 0)
                {
                    foreach (PostHeader data in HeaderList)
                    {
                        _httpClient.DefaultRequestHeaders.Add(data.HeaderKey, data.HeaderContent);
                    }
                }

                StringContent httpContent = new StringContent(Pramater, Encoding.UTF8);
                switch (ContentType)
                {
                    case ContentTypeEnum.urlencoded:
                        httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
                        break;
                    case ContentTypeEnum.json:
                        httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                        break;
                }
                //確保不會0秒，0秒就會一直等
                if (Timeout == 0)
                    Timeout = 90;

                _httpClient.Timeout = TimeSpan.FromSeconds(Timeout);
                response = await _httpClient.PostAsync(Url, httpContent);

            }
            catch (Exception ex)
            {
                await _LocalLog.WriteAsync(_className, _functionname, requestIDService, LogLevelEnum.Info, ex.ToString());
            }
            return response;
        }
    }
}
