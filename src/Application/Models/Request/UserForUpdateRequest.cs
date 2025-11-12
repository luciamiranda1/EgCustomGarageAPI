using System.ComponentModel.DataAnnotations;
using Domain.Entities;

namespace Application.Models.Request
{
    public class UserForUpdateRequest
    {
        public required string Name { get; set; }
        [EmailAddress]
        public required string Email { get; set; }
        public required string Password { get; set; }
        public UserRole Role { get; set; }
    }
}
