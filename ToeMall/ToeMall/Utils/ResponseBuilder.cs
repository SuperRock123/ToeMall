namespace ToeMall.Utils
{
    public class ResponseBuilder
    {
        private Response _response;

        public ResponseBuilder()
        {
            _response = new Response();
            _response.Data = null;
            
        }

        // 设置状态码
        public ResponseBuilder SetStatusCode(int? statusCode)
        {
            _response.StatusCode = statusCode;
            return this; // 返回当前对象，支持链式调用
        }

        // 设置消息
        public ResponseBuilder SetMessage(string? message)
        {
            _response.Message = message;
            return this;
        }

        // 设置数据
        public ResponseBuilder SetData(object? data)
        {
            _response.Data = data;
            return this;
        }

        // 获取最终的响应对象
        public Response Build()
        {
            return _response;
        }
    }
}
