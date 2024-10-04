using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DuitKu.Domain
{
    [Table("accounts", Schema = "public")]
    public class Account : BaseDomain
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

        [Required, Column("balance")]
        public decimal Balance { get; set; }

        [JsonIgnore]
        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}