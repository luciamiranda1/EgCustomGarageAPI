
namespace Application.Models.Request
{
    public class CreateCategoryRequest
    {
        public required string Name { get; set; } //qué campos espera la API cuando alguien crea o actualiza una categoría.lo que el usuario manda
    }
}