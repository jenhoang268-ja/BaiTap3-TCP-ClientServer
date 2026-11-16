using System;

namespace GameServer
{
    public class JsonErrorInfo
    {
        public DateTime Time { get; set; } = DateTime.Now;
        public string ClientAddress { get; set; } = "";
        public int ErrorCode { get; set; } = 400;
        public string Message { get; set; } = "";
        public string RawData { get; set; } = "";
    }
}