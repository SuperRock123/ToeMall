using Microsoft.EntityFrameworkCore;
using ToeMall.Data;
using ToeMall.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5227); // 👈 监听本机所有网卡
});
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// 从 appsettings.json 获取连接字符串
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
// 注意：使用 Pomelo MySQL 提供程序时，需要安装 Microsoft.EntityFrameworkCore.MySql 包
builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseMySql(
        connectionString,
        ServerVersion.AutoDetect(connectionString)
    ));
    // 添加 CORS 服务
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowSpecificOrigin",
                builder => builder
                    //.WithOrigins("http://localhost:8080") // 允许的来源
                    .AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod());
        });
var app = builder.Build();
// 启用 CORS
app.UseCors("AllowSpecificOrigin");
// 注意：使用中间件时，需要先添加中间件，再配置 HTTP 请求管道
app.UseMiddleware<RequestInterceptorMiddleware>();
//全局token验证中间件
app.UseMiddleware<TokenValidationMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
