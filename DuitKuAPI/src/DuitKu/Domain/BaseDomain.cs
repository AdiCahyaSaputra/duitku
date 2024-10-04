using System.ComponentModel.DataAnnotations.Schema;

namespace DuitKu.Domain
{
    public class BaseDomain
    {
        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }
    }
}