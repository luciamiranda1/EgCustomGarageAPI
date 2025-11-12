using System.ComponentModel.DataAnnotations;

namespace Application.Models.Request
{
    public class OrderForAddRequest
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "ProductId debe ser un entero positivo.")]
        public int ProductId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity debe ser al menos 1.")]
        public int Quantity { get; set; }
    }
}
