using System.ComponentModel.DataAnnotations;

namespace Application.Models.Request
{
    public class CreateProductRequest
    {
        [Required]
        [StringLength(100, ErrorMessage = "El nombre no puede superar los 100 caracteres.")]
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Stock { get; set; }

        [Required(ErrorMessage = "Debe especificarse una categoría.")]
        public int CategoryId { get; set; }
    }
}
