using System.Text;

namespace ToeMall.Utils
{
    public class RequestLogger
    {
        // 静态方法，用于打印请求参数
        public static async Task LogRequestAsync(HttpContext context)
        {
            var request = context.Request;

            // 打印请求方法和 URL
            Console.WriteLine($"Request Method: {request.Method}");
            Console.WriteLine($"Request URL: {request.Scheme}://{request.Host}{request.Path}{request.QueryString}");

            // 打印请求头
            Console.WriteLine("Request Headers:");
            foreach (var header in request.Headers)
            {
                Console.WriteLine($"  {header.Key}: {string.Join(", ", header.Value)}");
            }

            // 打印请求体（如果存在）
            if (request.ContentLength > 0)
            {
                request.EnableBuffering();  // 允许重复读取请求体
                using (var reader = new StreamReader(request.Body, Encoding.UTF8, leaveOpen: true))
                {
                    var body = await reader.ReadToEndAsync();
                    Console.WriteLine("Request Body:");
                    Console.WriteLine(body);
                    request.Body.Seek(0, SeekOrigin.Begin);  // 重置流位置，以便其他中间件可以读取请求体
                }
            }

            Console.WriteLine(); // 分隔行，便于查看
        }
    }
}
