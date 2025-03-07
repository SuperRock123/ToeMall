using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToeMall.Data;
using ToeMall.Models;
using ToeMall.Utils;

namespace ToeMall.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly MyDbContext _context;
        private readonly IConfiguration _configuration;

        public ProductsController(MyDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: api/Products/list
        // 获取商品列表，支持分页和排序
        [HttpGet("list")]
        public async Task<ActionResult<Response>> GetProducts(
            [FromQuery] int? page = 1,
            [FromQuery] int? pageSize = null,
            [FromQuery] int? categoryId = null,
            [FromQuery] string? sortBy = "createdAt", // createdAt, stock, category
            [FromQuery] bool? ascending = false)
        {
            // 从配置中获取分页参数
            var defaultPageSize = _configuration.GetValue<int>("CustomSettings:pagination:defaultPageSize");
            var maxPageSize = _configuration.GetValue<int>("CustomSettings:pagination:maxPageSize");

            // 验证并调整分页参数
            page = Math.Max(1, page ?? 1);
            pageSize = pageSize ?? defaultPageSize;
            pageSize = Math.Min(Math.Max(1, pageSize.Value), maxPageSize);

            // 构建查询
            var query = _context.Products
                .Include(p => p.Category)
                .AsQueryable();

            // 应用分类过滤
            if (categoryId.HasValue)
            {
                query = query.Where(p => p.CategoryId == categoryId.Value);
            }

            // 应用排序
            query = sortBy?.ToLower() switch
            {
                "stock" => ascending == true ? query.OrderBy(p => p.StockQuantity) : query.OrderByDescending(p => p.StockQuantity),
                "category" => ascending == true ? query.OrderBy(p => p.Category.Name) : query.OrderByDescending(p => p.Category.Name),
                _ => ascending == true ? query.OrderBy(p => p.CreatedAt) : query.OrderByDescending(p => p.CreatedAt)
            };

            // 计算总记录数
            var totalCount = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            // 获取分页数据
            var products = await query
                .Skip((page.Value - 1) * pageSize.Value)
                .Take(pageSize.Value)
                .Select(p => new
                {
                    p.ProductId,
                    p.Name,
                    p.Description,
                    p.Price,
                    p.StockQuantity,
                    p.Picture,
                    Category = new
                    {
                        p.Category.CategoryId,
                        p.Category.Name
                    },
                    p.CreatedAt,
                    p.UpdatedAt
                })
                .ToListAsync();

            // 构建分页信息
            var paginationInfo = new
            {
                CurrentPage = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = totalPages,
                CategoryId = categoryId,
                SortBy = sortBy,
                Ascending = ascending
            };

            // 构建响应数据
            var responseData = new
            {
                Products = products,
                Pagination = paginationInfo
            };

            return new ResponseBuilder()
                .SetStatusCode(200)
                .SetMessage("获取商品列表成功")
                .SetData(responseData)
                .Build();
        }

        // GET: api/Products/search
        // 搜索商品，支持分页和排序
        [HttpGet("search")]
        public async Task<ActionResult<Response>> SearchProducts(
            [FromQuery] string keyword,
            [FromQuery] int? page = 1,
            [FromQuery] int? pageSize = null,
            [FromQuery] int? categoryId = null,
            [FromQuery] string? sortBy = "createdAt",
            [FromQuery] bool? ascending = false)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {
                return new ResponseBuilder()
                    .SetStatusCode(400)
                    .SetMessage("搜索关键词不能为空")
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
            var query = _context.Products
                .Include(p => p.Category)
                .Where(p => 
                    EF.Functions.Like(p.Name, $"%{keyword}%") || 
                    EF.Functions.Like(p.Description, $"%{keyword}%") ||
                    EF.Functions.Like(p.Category.Name, $"%{keyword}%")
                );

            // 应用分类过滤
            if (categoryId.HasValue)
            {
                query = query.Where(p => p.CategoryId == categoryId.Value);
            }

            // 应用排序
            query = sortBy?.ToLower() switch
            {
                "stock" => ascending == true ? query.OrderBy(p => p.StockQuantity) : query.OrderByDescending(p => p.StockQuantity),
                "category" => ascending == true ? query.OrderBy(p => p.Category.Name) : query.OrderByDescending(p => p.Category.Name),
                _ => ascending == true ? query.OrderBy(p => p.CreatedAt) : query.OrderByDescending(p => p.CreatedAt)
            };

            // 计算总记录数
            var totalCount = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            // 获取分页数据
            var products = await query
                .Skip((page.Value - 1) * pageSize.Value)
                .Take(pageSize.Value)
                .Select(p => new
                {
                    p.ProductId,
                    p.Name,
                    p.Description,
                    p.Price,
                    p.StockQuantity,
                    p.Picture,
                    Category = new
                    {
                        p.Category.CategoryId,
                        p.Category.Name
                    },
                    p.CreatedAt,
                    p.UpdatedAt
                })
                .ToListAsync();

            // 构建分页信息
            var paginationInfo = new
            {
                CurrentPage = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = totalPages,
                Keyword = keyword,
                CategoryId = categoryId,
                SortBy = sortBy,
                Ascending = ascending
            };

            // 构建响应数据
            var responseData = new
            {
                Products = products,
                Pagination = paginationInfo
            };

            return new ResponseBuilder()
                .SetStatusCode(200)
                .SetMessage("搜索商品成功")
                .SetData(responseData)
                .Build();
        }

        // POST: api/Products/create
        // 添加商品（需要管理员权限）
        [HttpPost("create")]
        public async Task<ActionResult<Response>> CreateProduct([FromBody] Product product)
        {
            // 验证管理员权限
            var currentUser = HttpContext.Items["User"] as User;
            await RequestLogger.LogRequestAsync(HttpContext);
            // Console.WriteLine(currentUser.Role+" 当前用户"+currentUser.UserId);
            if (currentUser == null || currentUser.Role != "Admin")
            {
                return new ResponseBuilder()
                    .SetStatusCode(403)
                    .SetMessage("没有权限执行此操作")
                    .SetData(null)
                    .Build();
            }

            // 验证分类是否存在
            var category = await _context.Categories.FindAsync(product.CategoryId);
            if (category == null)
            {
                return new ResponseBuilder()
                    .SetStatusCode(400)
                    .SetMessage("商品分类不存在")
                    .SetData(null)
                    .Build();
            }

            // 设置创建时间和更新时间
            product.CreatedAt = DateTime.UtcNow;
            product.UpdatedAt = DateTime.UtcNow;
            product.StockQuantity = product.StockQuantity; // 使用传入的库存值
            product.Picture = product.Picture ?? string.Empty; // 设置默认空字符串

            try
            {
                _context.Products.Add(product);
                await _context.SaveChangesAsync();

                // 重新加载商品以获取完整的分类信息
                var createdProduct = await _context.Products
                    .Include(p => p.Category)
                    .FirstOrDefaultAsync(p => p.ProductId == product.ProductId);

                return new ResponseBuilder()
                    .SetStatusCode(201)
                    .SetMessage("商品创建成功")
                    .SetData(new
                    {
                        createdProduct.ProductId,
                        createdProduct.Name,
                        createdProduct.Description,
                        createdProduct.Price,
                        createdProduct.StockQuantity,
                        createdProduct.Picture,
                        Category = new
                        {
                            createdProduct.Category.CategoryId,
                            createdProduct.Category.Name
                        },
                        createdProduct.CreatedAt,
                        createdProduct.UpdatedAt
                    })
                    .Build();
            }
            catch (DbUpdateException)
            {
                return new ResponseBuilder()
                    .SetStatusCode(500)
                    .SetMessage("创建商品时发生错误")
                    .SetData(null)
                    .Build();
            }
        }

        // PUT: api/Products/update/{id}
        // 更新商品（需要管理员权限）
        [HttpPut("update/{id}")]
        public async Task<ActionResult<Response>> UpdateProduct(int id, [FromBody] Product product)
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

            // 检查商品是否存在
            var existingProduct = await _context.Products.FindAsync(id);
            if (existingProduct == null)
            {
                return new ResponseBuilder()
                    .SetStatusCode(404)
                    .SetMessage("商品不存在")
                    .SetData(null)
                    .Build();
            }

            // 验证分类是否存在
            var category = await _context.Categories.FindAsync(product.CategoryId);
            if (category == null)
            {
                return new ResponseBuilder()
                    .SetStatusCode(400)
                    .SetMessage("商品分类不存在")
                    .SetData(null)
                    .Build();
            }

            // 更新商品信息
            existingProduct.Name = product.Name;
            existingProduct.Description = product.Description;
            existingProduct.Price = product.Price;
            existingProduct.StockQuantity = product.StockQuantity;
            existingProduct.CategoryId = product.CategoryId;
            existingProduct.Picture = product.Picture;
            existingProduct.UpdatedAt = DateTime.UtcNow;

            try
            {
                await _context.SaveChangesAsync();

                return new ResponseBuilder()
                    .SetStatusCode(200)
                    .SetMessage("商品更新成功")
                    .SetData(new
                    {
                        existingProduct.ProductId,
                        existingProduct.Name,
                        existingProduct.Description,
                        existingProduct.Price,
                        existingProduct.StockQuantity,
                        existingProduct.Picture,
                        Category = new
                        {
                            existingProduct.Category.CategoryId,
                            existingProduct.Category.Name
                        },
                        existingProduct.CreatedAt,
                        existingProduct.UpdatedAt
                    })
                    .Build();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return new ResponseBuilder()
                        .SetStatusCode(404)
                        .SetMessage("商品不存在")
                        .SetData(null)
                        .Build();
                }
                else
                {
                    throw;
                }
            }
        }

        // DELETE: api/Products/delete/{id}
        // 删除商品（需要管理员权限）
        [HttpDelete("delete/{id}")]
        public async Task<ActionResult<Response>> DeleteProduct(int id)
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

            // 检查商品是否存在
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return new ResponseBuilder()
                    .SetStatusCode(404)
                    .SetMessage("商品不存在")
                    .SetData(null)
                    .Build();
            }

            try
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();

                return new ResponseBuilder()
                    .SetStatusCode(200)
                    .SetMessage("商品删除成功")
                    .SetData(new { DeletedProductId = id })
                    .Build();
            }
            catch (DbUpdateException)
            {
                return new ResponseBuilder()
                    .SetStatusCode(500)
                    .SetMessage("删除商品时发生错误")
                    .SetData(null)
                    .Build();
            }
        }

        // GET: api/Products/detail/{id}
        // 获取商品详情
        [HttpGet("detail/{id}")]
        public async Task<ActionResult<Response>> GetProduct(int id)
        {
            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.ProductId == id);

            if (product == null)
            {
                return new ResponseBuilder()
                    .SetStatusCode(404)
                    .SetMessage("商品不存在")
                    .SetData(null)
                    .Build();
            }

            // 构建商品详情响应
            var productDetail = new
            {
                product.ProductId,
                product.Name,
                product.Description,
                product.Price,
                product.StockQuantity,
                product.Picture,
                Category = new
                {
                    product.Category.CategoryId,
                    product.Category.Name,
                    product.Category.Description
                },
                product.CreatedAt,
                product.UpdatedAt
            };

            return new ResponseBuilder()
                .SetStatusCode(200)
                .SetMessage("获取商品详情成功")
                .SetData(productDetail)
                .Build();
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductId == id);
        }
    }
} 