using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToeMall.Data;
using ToeMall.Models;
using ToeMall.Utils;
using System.ComponentModel.DataAnnotations;

namespace ToeMall.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly MyDbContext _context;
        private readonly IConfiguration _configuration;

        public OrdersController(MyDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: api/Orders/list
        // 获取订单列表（管理员可以查看所有订单，普通用户只能查看自己的订单）
        [HttpGet("list")]
        public async Task<ActionResult<Response>> GetOrders(
            [FromQuery] int? page = 1,
            [FromQuery] int? pageSize = null,
            [FromQuery] string? status = null,
            [FromQuery] string? sortBy = "createdAt",
            [FromQuery] bool? ascending = false)
        {
            var currentUser = HttpContext.Items["User"] as User;
            if (currentUser == null)
            {
                return new ResponseBuilder()
                    .SetStatusCode(401)
                    .SetMessage("未授权访问")
                    .SetData(null)
                    .Build();
            }

            // 从配置中获取分页参数
            var defaultPageSize = _configuration.GetValue<int>("CustomSettings:pagination:defaultPageSize");
            var maxPageSize = _configuration.GetValue<int>("CustomSettings:pagination:maxPageSize");

            // 验证并调整分页参数
            page = Math.Max(1, page ?? 1);
            pageSize = pageSize ?? defaultPageSize;
            pageSize = Math.Min(Math.Max(1, pageSize.Value), maxPageSize);

            // 构建查询
            var query = _context.Orders
                .Include(o => o.Product)
                .Include(o => o.User)
                .AsQueryable();

            // 根据用户角色过滤订单
            if (currentUser.Role != "Admin")
            {
                query = query.Where(o => o.UserId == currentUser.UserId);
            }

            // 应用状态过滤
            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(o => o.OrderStatus == status);
            }

            // 应用排序
            query = sortBy?.ToLower() switch
            {
                "status" => ascending == true ? query.OrderBy(o => o.OrderStatus) : query.OrderByDescending(o => o.OrderStatus),
                "total" => ascending == true ? query.OrderBy(o => o.TotalPrice) : query.OrderByDescending(o => o.TotalPrice),
                _ => ascending == true ? query.OrderBy(o => o.CreatedAt) : query.OrderByDescending(o => o.CreatedAt)
            };

            // 计算总记录数
            var totalCount = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            // 获取分页数据
            var orders = await query
                .Skip((page.Value - 1) * pageSize.Value)
                .Take(pageSize.Value)
                .Select(o => new
                {
                    o.OrderId,
                    o.OrderStatus,
                    o.ProductName,
                    o.Price,
                    o.TotalPrice,
                    Product = new
                    {
                        o.Product.ProductId,
                        o.Product.Name,
                        o.Product.Price
                    },
                    User = new
                    {
                        o.User.UserId,
                        o.User.Username
                    },
                    o.Quantity,
                    o.CreatedAt,
                    o.UpdatedAt
                })
                .ToListAsync();

            // 构建分页信息
            var paginationInfo = new
            {
                CurrentPage = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = totalPages,
                Status = status,
                SortBy = sortBy,
                Ascending = ascending
            };

            // 构建响应数据
            var responseData = new
            {
                Orders = orders,
                Pagination = paginationInfo
            };

            return new ResponseBuilder()
                .SetStatusCode(200)
                .SetMessage("获取订单列表成功")
                .SetData(responseData)
                .Build();
        }

        public class CreateOrderDto
        {
            [Required]
            public int ProductId { get; set; }

            [Required]
            [Range(1, int.MaxValue, ErrorMessage = "数量必须大于0")]
            public int Quantity { get; set; }
        }

        // POST: api/Orders/create
        // 创建订单
        [HttpPost("create")]
        public async Task<ActionResult<Response>> CreateOrder([FromBody] CreateOrderDto orderDto)
        {
            var currentUser = HttpContext.Items["User"] as User;
            if (currentUser == null)
            {
                return new ResponseBuilder()
                    .SetStatusCode(401)
                    .SetMessage("未授权访问")
                    .SetData(null)
                    .Build();
            }

            // 验证商品是否存在
            var product = await _context.Products.FindAsync(orderDto.ProductId);
            if (product == null)
            {
                return new ResponseBuilder()
                    .SetStatusCode(400)
                    .SetMessage("商品不存在")
                    .SetData(null)
                    .Build();
            }

            // 验证库存是否充足
            if (product.StockQuantity < orderDto.Quantity)
            {
                return new ResponseBuilder()
                    .SetStatusCode(400)
                    .SetMessage("商品库存不足")
                    .SetData(null)
                    .Build();
            }

            // 创建新订单对象并自动填充相关信息
            var order = new Order
            {
                UserId = currentUser.UserId,
                ProductId = orderDto.ProductId,
                ProductName = product.Name,
                Quantity = orderDto.Quantity,
                Price = product.Price,
                TotalPrice = product.Price * orderDto.Quantity,
                OrderStatus = "Unpaid"
                // CreatedAt 和 UpdatedAt 会由数据库自动生成
            };

            try
            {
                // 减少商品库存
                product.StockQuantity -= order.Quantity;
                
                // 添加订单
                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                // 重新加载订单以获取完整信息
                var createdOrder = await _context.Orders
                    .Include(o => o.Product)
                    .Include(o => o.User)
                    .FirstOrDefaultAsync(o => o.OrderId == order.OrderId);

                return new ResponseBuilder()
                    .SetStatusCode(201)
                    .SetMessage("订单创建成功")
                    .SetData(new
                    {
                        createdOrder.OrderId,
                        createdOrder.OrderStatus,
                        createdOrder.ProductName,
                        createdOrder.Price,
                        createdOrder.TotalPrice,
                        Product = new
                        {
                            createdOrder.Product.ProductId,
                            createdOrder.Product.Name,
                            createdOrder.Product.Price
                        },
                        User = new
                        {
                            createdOrder.User.UserId,
                            createdOrder.User.Username
                        },
                        createdOrder.Quantity,
                        createdOrder.CreatedAt,
                        createdOrder.UpdatedAt
                    })
                    .Build();
            }
            catch (DbUpdateException)
            {
                return new ResponseBuilder()
                    .SetStatusCode(500)
                    .SetMessage("创建订单时发生错误")
                    .SetData(null)
                    .Build();
            }
        }

        // PUT: api/Orders/status/{id}
        // 更新订单状态（管理员可以更新所有订单，用户只能更新自己的订单）
        [HttpPut("status/{id}")]
        public async Task<ActionResult<Response>> UpdateOrderStatus(int id, [FromBody] OrderStatusUpdate statusUpdate)
        {
            var currentUser = HttpContext.Items["User"] as User;
            if (currentUser == null)
            {
                return new ResponseBuilder()
                    .SetStatusCode(401)
                    .SetMessage("未授权访问")
                    .SetData(null)
                    .Build();
            }

            var order = await _context.Orders
                .Include(o => o.Product)
                .Include(o => o.User)
                .FirstOrDefaultAsync(o => o.OrderId == id);

            if (order == null)
            {
                return new ResponseBuilder()
                    .SetStatusCode(404)
                    .SetMessage("订单不存在")
                    .SetData(null)
                    .Build();
            }

            // 验证权限
            if (currentUser.Role != "Admin" && order.UserId != currentUser.UserId)
            {
                return new ResponseBuilder()
                    .SetStatusCode(403)
                    .SetMessage("没有权限更新此订单")
                    .SetData(null)
                    .Build();
            }

            // 验证状态转换是否有效
            if (!IsValidStatusTransition(order.OrderStatus, statusUpdate.Status))
            {
                return new ResponseBuilder()
                    .SetStatusCode(400)
                    .SetMessage("无效的状态更新")
                    .SetData(null)
                    .Build();
            }

            try
            {
                order.OrderStatus = statusUpdate.Status;
                await _context.SaveChangesAsync();

                return new ResponseBuilder()
                    .SetStatusCode(200)
                    .SetMessage("订单状态更新成功")
                    .SetData(new
                    {
                        order.OrderId,
                        order.OrderStatus,
                        order.ProductName,
                        order.Price,
                        order.TotalPrice,
                        Product = new
                        {
                            order.Product.ProductId,
                            order.Product.Name,
                            order.Product.Price
                        },
                        User = new
                        {
                            order.User.UserId,
                            order.User.Username
                        },
                        order.Quantity,
                        order.CreatedAt,
                        order.UpdatedAt
                    })
                    .Build();
            }
            catch (DbUpdateException)
            {
                return new ResponseBuilder()
                    .SetStatusCode(500)
                    .SetMessage("更新订单状态时发生错误")
                    .SetData(null)
                    .Build();
            }
        }

        // GET: api/Orders/detail/{id}
        // 获取订单详情
        [HttpGet("detail/{id}")]
        public async Task<ActionResult<Response>> GetOrder(int id)
        {
            var currentUser = HttpContext.Items["User"] as User;
            if (currentUser == null)
            {
                return new ResponseBuilder()
                    .SetStatusCode(401)
                    .SetMessage("未授权访问")
                    .SetData(null)
                    .Build();
            }

            var order = await _context.Orders
                .Include(o => o.Product)
                .Include(o => o.User)
                .FirstOrDefaultAsync(o => o.OrderId == id);

            if (order == null)
            {
                return new ResponseBuilder()
                    .SetStatusCode(404)
                    .SetMessage("订单不存在")
                    .SetData(null)
                    .Build();
            }

            // 验证权限
            if (currentUser.Role != "Admin" && order.UserId != currentUser.UserId)
            {
                return new ResponseBuilder()
                    .SetStatusCode(403)
                    .SetMessage("没有权限查看此订单")
                    .SetData(null)
                    .Build();
            }

            var orderDetail = new
            {
                order.OrderId,
                order.OrderStatus,
                order.ProductName,
                order.Price,
                order.TotalPrice,
                Product = new
                {
                    order.Product.ProductId,
                    order.Product.Name,
                    order.Product.Price,
                    order.Product.Description,
                    order.Product.Picture
                },
                User = new
                {
                    order.User.UserId,
                    order.User.Username
                },
                order.Quantity,
                order.CreatedAt,
                order.UpdatedAt
            };

            return new ResponseBuilder()
                .SetStatusCode(200)
                .SetMessage("获取订单详情成功")
                .SetData(orderDetail)
                .Build();
        }

        private bool IsValidStatusTransition(string currentStatus, string newStatus)
        {
            // 定义有效的状态转换
            var validTransitions = new Dictionary<string, string[]>
            {
                { "Unpaid", new[] { "Paid", "Cancelled" } },
                { "Paid", new[] { "Shipping" } },
                { "Shipping", new[] { "Completed" } }
            };

            return validTransitions.ContainsKey(currentStatus) &&
                   validTransitions[currentStatus].Contains(newStatus);
        }
    }

    public class OrderStatusUpdate
    {
        public string Status { get; set; }
    }
    
    
    
} 