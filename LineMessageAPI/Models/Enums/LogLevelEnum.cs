namespace LineMessageAPI.Models.Enums
{
    public enum LogLevelEnum
    {
        Trace = 0,
        Debug = 1,//如果只DEBUG要看用這個
        Info = 2,//如果只是要做紀錄就用這個
        Warning = 3,//發生例外時用這個
        Error = 4,//發生錯誤時用這個
        Fatal = 5,//發生災害及錯誤用這個
    }
}
