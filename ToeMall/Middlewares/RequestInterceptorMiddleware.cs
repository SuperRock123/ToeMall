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
            // �������·���Ƿ�����ض�����
            if (context.Request.Path.StartsWithSegments("/abc"))
            {
                // �����ﴦ�����ص�����
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsync("Access to this route is forbidden.");
                return;
            }

            // ������һ���м��
            await _next(context);
        }
    }
}
