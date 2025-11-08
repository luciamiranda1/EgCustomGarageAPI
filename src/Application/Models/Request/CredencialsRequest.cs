
namespace Application.Models.Requests
{
    public class CredentialsRequest
    {
        public required string Name { get; set; }
        public required string Password { get; set; }
    }
}