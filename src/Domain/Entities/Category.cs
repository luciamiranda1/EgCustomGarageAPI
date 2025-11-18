using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Domain.Entities
{
    public class Category
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public required string Name { get; set; }
        public bool IsDeleted { get; set; } = false;

        // Relación 1 a muchos, una cat puede tener muchos prods.
        [JsonIgnore]
        public ICollection<Product> Products { get; set; } = new List<Product>();

    }
}