using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public required string Name { get; set; }
        [Required]
        [EmailAddress]
        public required string Email { get; set; }
        [Required]
        public required string Password { get; set; }

        public UserRole Role { get; set; } = UserRole.Client;
        public bool IsDeleted { get; set; }

        public ICollection<Order> Orders { get; set; } = new List<Order>();

    }

    public enum UserRole
    {
        Client,
        Admin
    }
}