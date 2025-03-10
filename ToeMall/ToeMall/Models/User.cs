using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ToeMall.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        [Required]
        [MaxLength(50)]
        public string? Username { get; set; }

        [Required]
        [MaxLength(100)]
        public string? Email { get; set; }

        [Required]
        [MaxLength(255)]
        public string? PasswordHash { get; set; }

        [Required]
        [MaxLength(10)]
        public string Role { get; set; } = "User"; // 默认值为 "User"

        public int PointsBalance { get; set; } = 20; // 默认值为 0

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // 默认值为当前时间

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow; // 默认值为当前时间

        public string? Avatar { get; set; }
    }
}
