using LineMessageAPI.Models.Enums;
using System.Text.Json.Serialization;

namespace LineMessageAPI.Models.RequestModels
{
    public class RequestModelBase
    {
        private RequestCodeEnum _requestcode { get; set; } = RequestCodeEnum.Default;
        [JsonPropertyName("code")]
        public int Code => (int)_requestcode;
        [JsonPropertyName("message")]
        public string RequestMessage => _requestcode.ToString();
        [JsonPropertyName("details")]
        public string Details { get; set; }
        [JsonPropertyName("data")]
        public object Data { get; set; }
        [JsonPropertyName("requestid")]
        public Guid RequestID { get; set; }
        [JsonPropertyName("utc8time")]
        public string UTC8Time => DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");

        public RequestModelBase() { }
        public RequestModelBase(RequestCodeEnum code, Guid requestID) { this._requestcode = code; this.RequestID = requestID; }
        public RequestModelBase(RequestCodeEnum code, Guid requestID, string details) { this._requestcode = code; this.RequestID = requestID; this.Details = details; }
        public RequestModelBase(RequestCodeEnum code, Guid requestID, object data, string details = null)
        {
            this._requestcode = code;
            this.RequestID = requestID;
            this.Details = details;
            this.Data = data;
        }
    }
}
