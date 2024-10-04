using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

using Microsoft.EntityFrameworkCore;

namespace DuitKu.Domain
{
    [Index(nameof(Email), IsUnique = true)]
    [Table("users", Schema = "public")]
    public class User : BaseDomain
    {
        [Column("id"), Key]
        public Guid Id { get; set; }

        [Required, Column("name")]
        public string Name { get; set; } = string.Empty;

        [Required, Column("email")]
        public string Email { get; set; } = string.Empty;

        [JsonIgnore] // Like $hidden in Laravel
        [Required, Column("password")]
        public string Password { get; set; } = string.Empty;

        [JsonIgnore]
        public ICollection<Transaction> Transactions { get; } = new List<Transaction>();

        [JsonIgnore]
        public ICollection<Account> Accounts { get; } = new List<Account>();

        [JsonIgnore]
        public ICollection<Category> Categories { get; } = new List<Category>();

        [JsonIgnore]
        public ICollection<SubCategory> SubCategories { get; } = new List<SubCategory>();
    }
}