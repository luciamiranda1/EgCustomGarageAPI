using System.ComponentModel.DataAnnotations;

namespace Application.Models.Request
{
    public class UserForAddRequest
    {
        public required string Name { get; set; }
        [EmailAddress]
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}