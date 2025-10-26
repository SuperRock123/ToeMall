using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ToeMall.Data;
using ToeMall.Utils;

namespace ToeMall.Middlewares
{
    public class TokenValidationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public TokenValidationMiddleware(RequestDelegate next, IServiceScopeFactory serviceScopeFactory)
        {
            _next = next;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // 检查是否需要跳过token验证的路径
            if (ShouldSkipTokenValidation(context.Request.Path))
            {
                await _next(context);
                return;
            }

            // 从请求头获取token
            var token = context.Request.Headers["Authorization"].ToString();
            // Console.WriteLine("token中间件:"+token);
            if (string.IsNullOrEmpty(token))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsJsonAsync(new ResponseBuilder()
                    .SetStatusCode(401)
                    .SetMessage("未提供认证token")
                    .SetData(null)
                    .Build());
                return;
            }

            // 使用作用域创建DbContext实例
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<MyDbContext>();

                // 查找token记录
                var tokenRecord = await dbContext.Tokens
                    .Include(t => t.User)
                    .FirstOrDefaultAsync(t => t.TokenId == token);

                if (tokenRecord == null)
                {
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsJsonAsync(new ResponseBuilder()
                        .SetStatusCode(401)
                        .SetMessage("无效的token")
                        .SetData(null)
                        .Build());
                    return;
                }

                // 检查token是否过期
                if (DateTime.UtcNow > tokenRecord.ExpiryTime)
                {
                    // 删除过期的token
                    dbContext.Tokens.Remove(tokenRecord);
                    await dbContext.SaveChangesAsync();

                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsJsonAsync(new ResponseBuilder()
                        .SetStatusCode(401)
                        .SetMessage("登录信息已过期，请重新登录")
                        .SetData(null)
                        .Build());
                    return;
                }

                // 将用户信息添加到HttpContext.Items中，供后续使用
                context.Items["User"] = tokenRecord.User;
                context.Items["Token"] = tokenRecord;
            }

            await _next(context);
        }

        private bool ShouldSkipTokenValidation(PathString path)
        {
            // 定义不需要token验证的路径
            var skipPaths = new[]
            {
                "/api/Users/login",
                "/api/Users/signup",
                "/swagger/index.html",
                "/api/products/list",
                "/api/products/search",
                "/api/products/detail",
                "/api/category/all",
                "/swagger"
            };

            return skipPaths.Any(p => path.StartsWithSegments(p));
        }
    }
}