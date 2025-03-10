using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace ToeMall.Middlewares
{
    public class RequestInterceptorMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestInterceptorMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // 检查请求路径是否符合特定规则
            if (context.Request.Path.StartsWithSegments("/abc"))
            {
                // 在这里处理拦截的请求
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsync("Access to this route is forbidden.");
                return;
            }

            // 调用下一个中间件
            await _next(context);
        }
    }
}
