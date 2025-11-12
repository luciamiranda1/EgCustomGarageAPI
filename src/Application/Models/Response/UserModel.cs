using System.Text.Json.Serialization;
using Domain.Entities;

namespace Application.Models.Response
{
    public class UserModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))] //en vez de ser 1 y 2, transforma en user y admin, este es para la response y el otro para la bd
        public UserRole Role { get; set; }
    }
}