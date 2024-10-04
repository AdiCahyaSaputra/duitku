using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DuitKu.Domain
{
    [Table("categories", Schema = "public")]
    public class Category : BaseDomain
    {
        [Column("id"), Key]
        public Guid Id { get; set; }

        [JsonIgnore]
        [Required, Column("user_id")]
        public Guid UserId { get; set; }

        [JsonIgnore]
        public User User { get; set; } = null!;

        [Required, Column("name")]
        public string Name { get; set; } = string.Empty;

        [JsonIgnore]
        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

        [JsonIgnore]
        public ICollection<SubCategory> SubCategories { get; } = new List<SubCategory>();
    }
}