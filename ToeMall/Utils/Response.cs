namespace ToeMall.Utils
{
    public class Response
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }

        // 可以根据需要添加更多字段
    }
}
