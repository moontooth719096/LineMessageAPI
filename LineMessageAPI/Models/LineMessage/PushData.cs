namespace LineMessageAPI.Models.LineMessage
{
    public class PushData
    {
        public string to { get; set; }
        public Message[]? messages { get; set; }
    }

    public class Message
    {
        public string type { get; set; }
        public string text { get; set; }
    }

}
