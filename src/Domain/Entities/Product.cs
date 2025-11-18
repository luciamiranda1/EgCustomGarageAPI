using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public required string Name { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public bool IsDeleted { get; set; } = false; // baja lógica

        // Relación con categoría 
        public int CategoryId { get; set; }  // Foreign Key
        public Category? Category { get; set; }

        // Constructor vacío y otro con parámetros
        public Product() { }
        public Product(string name, decimal price, int stock, int categoryId)
        {
            Name = name;
            Price = price;
            Stock = stock;
            CategoryId = categoryId;

        }
    }
}