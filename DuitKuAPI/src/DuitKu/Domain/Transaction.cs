using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DuitKu.Domain
{
    [Table("transactions", Schema = "public")]
    public class Transaction : BaseDomain
    {
        [Column("id"), Key]
        public Guid Id { get; set; }

        [Required, Column("user_id")]
        public Guid UserId { get; set; }

        [JsonIgnore]
        public User User { get; set; } = null!;

        [Required, Column("account_id")]
        public Guid AccountId { get; set; }

        [JsonIgnore]
        public Account Account { get; set; } = null!;

        [Required, Column("category_id")]
        public Guid CategoryId { get; set; } 

        [JsonIgnore]
        public Category Category { get; set; } = null!;

        [Column("sub_category_id")]
        public Guid ? SubCategoryId { get; set; } 

        [JsonIgnore]
        public SubCategory SubCategory { get; set; } = null!;

        [Column("description")]
        public string Description { get; set; } = string.Empty;

        [Column("amount")]
        public decimal Amount { get; set; } 

        [Column("date")]
        public DateTime Date { get; set; }
    }
}