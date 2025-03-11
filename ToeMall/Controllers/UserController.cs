using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToeMall.Data;
using ToeMall.Models;
using ToeMall.Utils;
namespace ToeMall.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly MyDbContext _context;
        private readonly IConfiguration _configuration;

        public UsersController(MyDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: api/Users
        // /api/Users?page=2&pageSize=20
        [HttpGet]
        public async Task<ActionResult<Response>> GetUsers([FromQuery] int? page = 1, [FromQuery] int? pageSize = null)
        {
            // 从配置中获取分页参数
            var defaultPageSize = _configuration.GetValue<int>("CustomSettings:pagination:defaultPageSize");
            var maxPageSize = _configuration.GetValue<int>("CustomSettings:pagination:maxPageSize");

            // 验证并调整分页参数
            page = Math.Max(1, page ?? 1);
            pageSize = pageSize ?? defaultPageSize;
            pageSize = Math.Min(Math.Max(1, pageSize.Value), maxPageSize);

            // 计算总记录数
            var totalCount = await _context.Users.CountAsync();
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            // 获取分页数据
            var users = await _context.Users
                .OrderByDescending(u => u.CreatedAt)
                .Skip((page.Value - 1) * pageSize.Value)
                .Take(pageSize.Value)
                .Select(u => new
                {
                    u.UserId,
                    u.Username,
                    u.Email,
                    u.Role,
                    u.PointsBalance,
                    u.CreatedAt,
                    u.UpdatedAt,
                    u.Avatar
                })
                .ToListAsync();

            // 构建分页信息
            var paginationInfo = new
            {
                CurrentPage = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = totalPages
            };

            // 构建响应数据
            var responseData = new
            {
                Users = users,
                Pagination = paginationInfo
            };

            return new ResponseBuilder()
                .SetStatusCode(200)
                .SetMessage("获取用户列表成功")
                .SetData(responseData)
                .Build();
        }
        // POST: api/Users/login
        [HttpPost("login")]
        public async Task<ActionResult<Response>> Login([FromBody] LoginRequest request)
        {
            // 验证用户名和密码
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == request.Username);

            if (user == null || !VerifyPassword(request.Password, user.PasswordHash))
            {
                return new ResponseBuilder()
                    .SetStatusCode(401)
                    .SetMessage("用户名或密码错误")
                    .SetData(null)
                    .Build();
            }

            try
            {
                // 删除该用户的所有现有token
                var existingTokens = await _context.Tokens
                    .Where(t => t.UserId == user.UserId)
                    .ToListAsync();

                if (existingTokens.Any())
                {
                    _context.Tokens.RemoveRange(existingTokens);
                    await _context.SaveChangesAsync();
                }

                // 从配置文件获取token过期时间（天数）
                var expiryDays = _configuration.GetValue<int>("CustomSettings:token:expiryTime");
                var expiryTime = DateTime.UtcNow.AddDays(expiryDays);
                var tokenId = HashEncoding.ComputeHash(user.UserId + expiryTime.ToString());
                
                var token = new Token
                {
                    TokenId = tokenId,
                    UserId = user.UserId,
                    LoginTime = DateTime.UtcNow,
                    ExpiryTime = expiryTime
                };

                _context.Tokens.Add(token);
                await _context.SaveChangesAsync();

                return new ResponseBuilder()
                    .SetStatusCode(200)
                    .SetMessage("登录成功")
                    .SetData(new
                    {
                        user.UserId,
                        user.Username,
                        user.Email,
                        user.Role,
                        user.Avatar,
                        Token = tokenId
                    })
                    .Build();
            }
            catch (Exception)
            {
                return new ResponseBuilder()
                    .SetStatusCode(500)
                    .SetMessage("登录过程中发生错误")
                    .SetData(null)
                    .Build();
            }
        }

        private bool VerifyPassword(string inputPassword, string storedHash)
        {
            var inputHash = HashEncoding.ComputeHash(inputPassword);
            return inputHash == storedHash;
        }

        private string GenerateToken()
        {
            // 使用用户信息和时间戳生成token
            var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
            var randomPart = Guid.NewGuid().ToString("N");
            var combinedString = $"{timestamp}{randomPart}";
            
            // 使用SHA256生成token
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var bytes = System.Text.Encoding.UTF8.GetBytes(combinedString);
                var hash = sha256.ComputeHash(bytes);
                return BitConverter.ToString(hash).Replace("-", "").ToLower();
            }
        }

        // POST: api/users/signup
        [HttpPost("signup")]
        public async Task<ActionResult<Response>> PostUser(User user)
        {
            //User表的UserId自增
            user.UserId = 0;
            user.Role = "user";
            if (UserExists(user.Username))
            {
                 return new ResponseBuilder().SetStatusCode(400).SetMessage("用户名已经存在").SetData(null).Build();
            }
            user.PasswordHash = HashEncoding.ComputeHash(user.PasswordHash);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return new ResponseBuilder().SetStatusCode(200).SetMessage("注册成功").SetData(null).Build();
        }
        // PUT: api/Users/update
        // 修改用户信息
        [HttpPut("update")]
        public async Task<ActionResult<Response>> PutUser([FromBody] User user)
        {
            // 1. 获取当前用户（经过中间件注入）
            var currentUser = HttpContext.Items["User"] as User;
            if (currentUser == null)
            {
                return new ResponseBuilder()
                    .SetStatusCode(401)
                    .SetMessage("未授权访问")
                    .SetData(null)
                    .Build();
            }

            // 2. 检查待修改用户是否存在
            var existingUser = await _context.Users.FindAsync(user.UserId);
            if (existingUser == null)
            {
                return new ResponseBuilder()
                    .SetStatusCode(404)
                    .SetMessage("用户不存在")
                    .SetData(null)
                    .Build();
            }

            // 3. 权限验证：普通用户只能修改自己的信息，管理员可以修改所有用户的信息
            if (currentUser.Role != "Admin" && currentUser.UserId != user.UserId)
            {
                return new ResponseBuilder()
                    .SetStatusCode(403)
                    .SetMessage("没有权限修改其他用户的信息")
                    .SetData(null)
                    .Build();
            }

            // 4. 数据验证：检查用户名是否已被其他用户使用
            bool usernameExists = await _context.Users
                .AnyAsync(u => u.Username == user.Username && u.UserId != user.UserId);
            if (usernameExists)
            {
                return new ResponseBuilder()
                    .SetStatusCode(400)
                    .SetMessage("用户名已被使用")
                    .SetData(null)
                    .Build();
            }

            // 5. 更新用户信息
            existingUser.Username = user.Username;
            existingUser.Email = user.Email;
            existingUser.Avatar = user.Avatar;
            existingUser.UpdatedAt = DateTime.UtcNow;

            // 只有管理员可以修改积分和角色
            if (currentUser.Role == "Admin")
            {
                existingUser.Role = user.Role;
                existingUser.PointsBalance = user.PointsBalance;
            }

            // 如果提供了新密码，则更新密码（注意：最好是要求前端传递新密码，再进行哈希计算）
            if (!string.IsNullOrEmpty(user.PasswordHash))
            {
                existingUser.PasswordHash = HashEncoding.ComputeHash(user.PasswordHash);
            }

            try
            {
                // 6. 删除该用户所有的 Token 记录，确保修改后需要重新登录
                var userTokens = _context.Tokens.Where(t => t.UserId == user.UserId);
                _context.Tokens.RemoveRange(userTokens);

                // 7. 保存所有修改（用户信息更新 + Token 删除）
                await _context.SaveChangesAsync();

                // 8. 返回更新后的用户信息（去除敏感信息）
                var updatedUserInfo = new
                {
                    existingUser.UserId,
                    existingUser.Username,
                    existingUser.Email,
                    existingUser.Role,
                    existingUser.PointsBalance,
                    existingUser.CreatedAt,
                    existingUser.UpdatedAt,
                    existingUser.Avatar
                };

                return new ResponseBuilder()
                    .SetStatusCode(200)
                    .SetMessage("用户信息更新成功，请重新登录")
                    .SetData(updatedUserInfo)
                    .Build();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(user.UserId))
                {
                    return new ResponseBuilder()
                        .SetStatusCode(404)
                        .SetMessage("用户不存在")
                        .SetData(null)
                        .Build();
                }
                else
                {
                    throw;
                }
            }
        }


        // DELETE: api/Users/{id}
        // 管理员权限
        [HttpDelete("{id}")]
        public async Task<ActionResult<Response>> DeleteUser(int id)
        {
            // 获取当前用户（管理员）
            var currentUser = HttpContext.Items["User"] as User;
            if (currentUser == null || currentUser.Role != "Admin")
            {
                return new ResponseBuilder()
                    .SetStatusCode(403)
                    .SetMessage("没有权限执行此操作")
                    .SetData(null)
                    .Build();
            }

            // 检查要删除的用户是否存在
            var userToDelete = await _context.Users.FindAsync(id);
            if (userToDelete == null)
            {
                return new ResponseBuilder()
                    .SetStatusCode(404)
                    .SetMessage("用户不存在")
                    .SetData(null)
                    .Build();
            }

            // 防止管理员删除自己
            if (userToDelete.UserId == currentUser.UserId)
            {
                return new ResponseBuilder()
                    .SetStatusCode(400)
                    .SetMessage("不能删除自己的账号")
                    .SetData(null)
                    .Build();
            }

            try
            {
                // 删除用户的所有token记录
                var userTokens = await _context.Tokens
                    .Where(t => t.UserId == id)
                    .ToListAsync();
                _context.Tokens.RemoveRange(userTokens);

                // 删除用户
                _context.Users.Remove(userToDelete);
                await _context.SaveChangesAsync();

                return new ResponseBuilder()
                    .SetStatusCode(200)
                    .SetMessage("用户删除成功")
                    .SetData(new { DeletedUserId = id })
                    .Build();
            }
            catch (DbUpdateException ex)
            {
                // 记录错误日志
                return new ResponseBuilder()
                    .SetStatusCode(500)
                    .SetMessage("删除用户时发生错误")
                    .SetData(null)
                    .Build();
            }
        }

        // GET: api/Users/info
        [HttpGet("info")]
        public ActionResult<Response> GetUserInfo()
        {
            // 从HttpContext.Items中获取用户信息
            var user = HttpContext.Items["User"] as User;
            if (user == null)
            {
                return new ResponseBuilder()
                    .SetStatusCode(401)
                    .SetMessage("获取用户信息失败")
                    .SetData(null)
                    .Build();
            }

            // 返回不敏感的用户信息
            var userInfo = new
            {
                user.UserId,
                user.Username,
                user.Email,
                user.Role,
                user.PointsBalance,
                user.CreatedAt,
                user.UpdatedAt,
                user.Avatar
            };

            return new ResponseBuilder()
                .SetStatusCode(200)
                .SetMessage("获取用户信息成功")
                .SetData(userInfo)
                .Build();
        }

        // GET: api/Users/search
        // /api/Users/search?username=test&page=1&pageSize=10
        [HttpGet("search")]
        public async Task<ActionResult<Response>> SearchUsers(
            [FromQuery] string username,
            [FromQuery] int? page = 1,
            [FromQuery] int? pageSize = null)
        {
            if (string.IsNullOrWhiteSpace(username))
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
            var query = _context.Users
                .Where(u => u.Username.Contains(username));

            // 计算总记录数
            var totalCount = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            // 获取分页数据
            var users = await query
                .OrderByDescending(u => u.CreatedAt)
                .Skip((page.Value - 1) * pageSize.Value)
                .Take(pageSize.Value)
                .Select(u => new
                {
                    u.UserId,
                    u.Username,
                    u.Email,
                    u.Role,
                    u.PointsBalance,
                    u.CreatedAt,
                    u.UpdatedAt,
                    u.Avatar
                })
                .ToListAsync();

            // 构建分页信息
            var paginationInfo = new
            {
                CurrentPage = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = totalPages,
                SearchTerm = username
            };

            // 构建响应数据
            var responseData = new
            {
                Users = users,
                Pagination = paginationInfo
            };

            return new ResponseBuilder()
                .SetStatusCode(200)
                .SetMessage("搜索用户成功")
                .SetData(responseData)
                .Build();
        }
        [HttpPost("admin/adduser")]
        public async Task<ActionResult<Response>> AddUserByAdmin([FromBody] User user)
        {
            // 获取当前用户（管理员）
            var currentUser = HttpContext.Items["User"] as User;
            if (currentUser == null || currentUser.Role != "Admin")
            {
                return new ResponseBuilder()
                    .SetStatusCode(403)
                    .SetMessage("没有权限执行此操作")
                    .SetData(null)
                    .Build();
            }

            // 检查用户名是否已存在
            if (UserExists(user.Username))
            {
                return new ResponseBuilder()
                    .SetStatusCode(400)
                    .SetMessage("用户名已经存在")
                    .SetData(null)
                    .Build();
            }

            // 设置默认角色和密码哈希
            user.UserId = 0; // 确保 UserId 自增
            user.Role = user.Role ?? "user"; // 如果未指定角色，默认为 "user"
            user.PasswordHash = HashEncoding.ComputeHash(user.PasswordHash);

            // 添加用户到数据库
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new ResponseBuilder()
                .SetStatusCode(200)
                .SetMessage("用户添加成功")
                .SetData(new
                {
                    user.UserId,
                    user.Username,
                    user.Email,
                    user.Role,
                    user.CreatedAt,
                    user.UpdatedAt,
                    user.Avatar
                })
                .Build();
        }
        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }
        private bool UserExists(string username)
        {
            return _context.Users.Any(e => e.Username == username);
        }
    }

    
    //用于解析请求的临时类
    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
