using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToeMall.Data;
using ToeMall.Models;
using ToeMall.Utils;

namespace ToeMall.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly MyDbContext _context;
    private readonly IConfiguration _configuration;
        public CategoryController(MyDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // POST: api/Category/create
        // 添加分类（需要管理员权限）
        [HttpPost("create")]
        public async Task<ActionResult<Response>> CreateCategory([FromBody] Category category)
        {
            var currentUser = HttpContext.Items["User"] as User;
            if (currentUser == null || currentUser.Role != "Admin")
            {
                return new ResponseBuilder()
                    .SetStatusCode(403)
                    .SetMessage("没有权限执行此操作")
                    .SetData(null)
                    .Build();
            }

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return new ResponseBuilder()
                .SetStatusCode(201)
                .SetMessage("分类创建成功")
                .SetData(category)
                .Build();
        }

        // PUT: api/Category/update/{id}
        // 更新分类（需要管理员权限）
        [HttpPut("update/{id}")]
        public async Task<ActionResult<Response>> UpdateCategory(int id, [FromBody] Category category)
        {
            var currentUser = HttpContext.Items["User"] as User;
            if (currentUser == null || currentUser.Role != "Admin")
            {
                return new ResponseBuilder()
                    .SetStatusCode(403)
                    .SetMessage("没有权限执行此操作")
                    .SetData(null)
                    .Build();
            }

            var existingCategory = await _context.Categories.FindAsync(id);
            if (existingCategory == null)
            {
                return new ResponseBuilder()
                    .SetStatusCode(404)
                    .SetMessage("分类不存在")
                    .SetData(null)
                    .Build();
            }

            existingCategory.Name = category.Name;
            existingCategory.Description = category.Description;

            await _context.SaveChangesAsync();

            return new ResponseBuilder()
                .SetStatusCode(200)
                .SetMessage("分类更新成功")
                .SetData(existingCategory)
                .Build();
        }

        // DELETE: api/Category/delete/{id}
        // 删除分类（需要管理员权限）
        [HttpDelete("delete/{id}")]
        public async Task<ActionResult<Response>> DeleteCategory(int id)
        {
            var currentUser = HttpContext.Items["User"] as User;
            if (currentUser == null || currentUser.Role != "Admin")
            {
                return new ResponseBuilder()
                    .SetStatusCode(403)
                    .SetMessage("没有权限执行此操作")
                    .SetData(null)
                    .Build();
            }

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return new ResponseBuilder()
                    .SetStatusCode(404)
                    .SetMessage("分类不存在")
                    .SetData(null)
                    .Build();
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return new ResponseBuilder()
                .SetStatusCode(200)
                .SetMessage("分类删除成功")
                .SetData(new { DeletedCategoryId = id })
                .Build();
        }

        // GET: api/Category/{id}/products
        // 按分类分页查询商品，无权限验证
        [HttpGet("{id}/products")]
        public async Task<ActionResult<Response>> GetProductsByCategory(
            int id,
            [FromQuery] int? page = 1,
            [FromQuery] int? pageSize = null,
            [FromQuery] string? sortBy = "createdAt",
            [FromQuery] bool? ascending = false)
        {
                       // 从配置中获取分页参数
            var defaultPageSize = _configuration.GetValue<int>("CustomSettings:pagination:defaultPageSize");
            var maxPageSize = _configuration.GetValue<int>("CustomSettings:pagination:maxPageSize");

            page = Math.Max(1, page ?? 1);
            pageSize = pageSize ?? defaultPageSize;
            pageSize = Math.Min(Math.Max(1, pageSize.Value), maxPageSize);

            var query = _context.Products
                .Include(p => p.Category)
                .Where(p => p.CategoryId == id)
                .AsQueryable();

            query = sortBy?.ToLower() switch
            {
                "stock" => ascending == true ? query.OrderBy(p => p.StockQuantity) : query.OrderByDescending(p => p.StockQuantity),
                "category" => ascending == true ? query.OrderBy(p => p.Category.Name) : query.OrderByDescending(p => p.Category.Name),
                _ => ascending == true ? query.OrderBy(p => p.CreatedAt) : query.OrderByDescending(p => p.CreatedAt)
            };

            var totalCount = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

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

            var paginationInfo = new
            {
                CurrentPage = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = totalPages,
                CategoryId = id,
                SortBy = sortBy,
                Ascending = ascending
            };

            var responseData = new
            {
                Products = products,
                Pagination = paginationInfo
            };

            return new ResponseBuilder()
                .SetStatusCode(200)
                .SetMessage("获取分类商品列表成功")
                .SetData(responseData)
                .Build();
        }

        // GET: api/Category/all
        // 返回所有分类数据，无权限验证
        [HttpGet("all")]
        public async Task<ActionResult<Response>> GetAllCategories()
        {
            var categories = await _context.Categories.ToListAsync();

            return new ResponseBuilder()
                .SetStatusCode(200)
                .SetMessage("获取所有分类成功")
                .SetData(categories)
                .Build();
        }
    }
}