using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ToeMall.Models
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductId { get; set; }

        [Required]
        [MaxLength(100)]
        public string? Name { get; set; }

        public string? Description { get; set; }

        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal Price { get; set; }

        public int StockQuantity { get; set; } = 0; // 默认值为 0

        [ForeignKey("Category")]
        public int CategoryId { get; set; }

        public virtual Category? Category { get; set; } // 导航属性，标记为可空

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // 默认值为当前时间

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow; // 默认值为当前时间

        [MaxLength(255)]
        public string? Picture { get; set; } // 商品图片URL
    }
}
