using System.ComponentModel;

namespace LineMessageAPI.Models.Enums
{
    public enum RequestCodeEnum
    {
        [Description("預設值")]
        Default = 0,
        [Description("成功")]
        Success = 1,
        [Description("參數錯誤")]
        ParameterError = -1,
        [Description("發生不可預期例外")]
        Exception = -9999
    }
}
