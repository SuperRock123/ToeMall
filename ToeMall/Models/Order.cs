using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToeMall.Models
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderId { get; set; }

        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }

        [Required]
        [ForeignKey("Product")]
        public int ProductId { get; set; }

        [MaxLength(100)]
        public string? ProductName { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; } = 1;

        [Column(TypeName = "decimal(10, 2)")]
        public decimal? Price { get; set; }

        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal TotalPrice { get; set; }

        [Required]
        [Column(TypeName = "enum('Unpaid', 'Paid', 'Shipping', 'Cancelled')")]
        public string OrderStatus { get; set; } = "Unpaid";

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // 导航属性
        public virtual User User { get; set; }
        public virtual Product Product { get; set; }
    }
}
