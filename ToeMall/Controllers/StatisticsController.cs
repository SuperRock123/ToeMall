using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToeMall.Data;
using ToeMall.Models;
using ToeMall.Utils;

namespace ToeMall.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatisticsController : ControllerBase
    {
        private readonly MyDbContext _context;
        private readonly IConfiguration _configuration;

        public StatisticsController(MyDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // 获取用户统计信息
        [HttpGet("users")]
        public async Task<ActionResult<Response>> GetUserStatistics()
        {
            // 验证管理员权限
            var currentUser = HttpContext.Items["User"] as User;
            if (currentUser == null || currentUser.Role != "Admin")
            {
                return new ResponseBuilder()
                    .SetStatusCode(403)
                    .SetMessage("没有权限执行此操作")
                    .SetData(null)
                    .Build();
            }

            var stats = new
            {
                TotalUsers = await _context.Users.CountAsync(),
                ActiveUsers = await _context.Tokens
                    .Where(t => t.ExpiryTime > DateTime.UtcNow)
                    .CountAsync(),
                NewUsersToday = await _context.Users
                    .Where(u => u.CreatedAt.Date == DateTime.UtcNow.Date)
                    .CountAsync()
            };

            return new ResponseBuilder()
                .SetStatusCode(200)
                .SetMessage("获取用户统计信息成功")
                .SetData(stats)
                .Build();
        }

        // 获取产品统计信息
        [HttpGet("products")]
        public async Task<ActionResult<Response>> GetProductStatistics()
        {
            // 验证管理员权限
            var currentUser = HttpContext.Items["User"] as User;
            if (currentUser == null || currentUser.Role != "Admin")
            {
                return new ResponseBuilder()
                    .SetStatusCode(403)
                    .SetMessage("没有权限执行此操作")
                    .SetData(null)
                    .Build();
            }

            var stats = new
            {
                TotalProducts = await _context.Products.CountAsync(),
                LowStockProducts = await _context.Products
                    .Where(p => p.StockQuantity < 10)
                    .CountAsync(),
                CategoriesCount = await _context.Categories.CountAsync()
            };

            return new ResponseBuilder()
                .SetStatusCode(200)
                .SetMessage("获取产品统计信息成功")
                .SetData(stats)
                .Build();
        }

        [HttpGet("orders")]
        public async Task<ActionResult<Response>> GetOrderStatistics()
        {
            // 验证管理员权限
            var currentUser = HttpContext.Items["User"] as User;
            if (currentUser == null || currentUser.Role != "Admin")
            {
                return new ResponseBuilder()
                    .SetStatusCode(403)
                    .SetMessage("没有权限执行此操作")
                    .SetData(null)
                    .Build();
            }

            var stats = new
            {
                TotalOrders = await _context.Orders.CountAsync(),
                TotalRevenue = await _context.Orders
                    .Where(o => o.OrderStatus == "Paid" || o.OrderStatus == "Shipping")
                    .SumAsync(o => o.TotalPrice),
                PendingOrders = await _context.Orders
                    .Where(o => o.OrderStatus == "Unpaid")
                    .CountAsync(),
                TodayOrders = await _context.Orders
                    .Where(o => o.CreatedAt.Date == DateTime.UtcNow.Date)
                    .CountAsync(),
                AverageOrderValue = await _context.Orders
                    .Where(o => o.OrderStatus == "Paid")
                    .AverageAsync(o => o.TotalPrice),
                DailyStats = await GetDailyOrderStats()
            };

            return new ResponseBuilder()
                .SetStatusCode(200)
                .SetMessage("获取订单统计信息成功")
                .SetData(stats)
                .Build();
        }

        private async Task<List<DailyOrderStats>> GetDailyOrderStats()
        {
            var dailyStats = new List<DailyOrderStats>();
            var startDate = DateTime.UtcNow.Date;

            for (int i = 0; i < 7; i++)
            {
                var date = startDate.AddDays(-i);
                var generatedOrdersCount = await _context.Orders
                    .Where(o => o.CreatedAt.Date == date)
                    .CountAsync();
                var completedOrdersCount = await _context.Orders
                    .Where(o => o.CreatedAt.Date == date && o.OrderStatus == "Paid")
                    .CountAsync();
                var totalRevenue = await _context.Orders
                    .Where(o => o.CreatedAt.Date == date && o.OrderStatus == "Paid")
                    .SumAsync(o => o.TotalPrice);

                dailyStats.Add(new DailyOrderStats
                {
                    Date = date,
                    GeneratedOrdersCount = generatedOrdersCount,
                    CompletedOrdersCount = completedOrdersCount,
                    TotalRevenue = totalRevenue
                });
            }

            return dailyStats;
        }

        public class DailyOrderStats
        {
            public DateTime Date { get; set; }
            public int GeneratedOrdersCount { get; set; }
            public int CompletedOrdersCount { get; set; }
            public decimal TotalRevenue { get; set; }
        }

        // 获取综合仪表盘统计
        [HttpGet("dashboard")]
        public async Task<ActionResult<Response>> GetDashboardStatistics()
        {
            // 验证管理员权限
            var currentUser = HttpContext.Items["User"] as User;
            if (currentUser == null || currentUser.Role != "Admin")
            {
                return new ResponseBuilder()
                    .SetStatusCode(403)
                    .SetMessage("没有权限执行此操作")
                    .SetData(null)
                    .Build();
            }

            var stats = new
            {
                TotalUsers = await _context.Users.CountAsync(),
                TotalOrders = await _context.Orders.CountAsync(),
                TotalRevenue = await _context.Orders
                    .Where(o => o.OrderStatus == "Paid" || o.OrderStatus == "Shipping")
                    .SumAsync(o => o.TotalPrice),
                TotalProducts = await _context.Products.CountAsync(),
                RecentOrders = await _context.Orders
                    .OrderByDescending(o => o.CreatedAt)
                    .Take(5)
                    .Select(o => new
                    {
                        o.OrderId,
                        o.ProductName,
                        o.TotalPrice,
                        o.OrderStatus,
                        o.CreatedAt
                    })
                    .ToListAsync()
            };

            return new ResponseBuilder()
                .SetStatusCode(200)
                .SetMessage("获取仪表盘统计信息成功")
                .SetData(stats)
                .Build();
        }
    }
}