namespace LineMessageAPI.Services
{
    public class RequestIDService
    {
        public Guid RequestID { get; set; }
        public string ClientIP { get; set; }
        public RequestIDService(IHttpContextAccessor httpContextAccessor)
        {
            RequestID = Guid.NewGuid();
            ClientIP = Userip_Get(httpContextAccessor.HttpContext);
        }

        public string Userip_Get(HttpContext context)
        {
            string IP = string.Empty;
            try
            {

                if (!string.IsNullOrEmpty(context.Request.Headers["X-Forwarded-For"]))
                {
                    IP = context.Request.Headers["X-Forwarded-For"];
                }
                else if (!string.IsNullOrEmpty(context.Request.Headers["MS_HttpContext"]))
                {
                    IP = context.Request.Headers["MS_HttpContext"];
                }
                else
                {
                    IP = context.Connection.RemoteIpAddress.ToString();

                    if (context.Connection.RemoteIpAddress.IsIPv4MappedToIPv6)
                    {
                        IP = context.Connection.RemoteIpAddress.MapToIPv4().ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            if (!string.IsNullOrEmpty(IP))
            {
                IP = IP.Split(',')[0].Trim();
            }
            return IP;
        }
    }
}
