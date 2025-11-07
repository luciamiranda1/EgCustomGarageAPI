using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int ProductId { get; set; } 
        public Product? Product { get; set; }
        public int ClientId { get; set; }
        public User? Client { get; set; }
        public int Quantity { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;

    }
}