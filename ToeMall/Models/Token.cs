using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToeMall.Models
{
    public class Token
    {
        [Key]
        [MaxLength(255)]
        public string? TokenId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public DateTime LoginTime { get; set; }

        [Required]
        public DateTime ExpiryTime { get; set; }

        [ForeignKey(nameof(UserId))]  // ��ȷָ�����
        public User? User { get; set; }
    }
}
