namespace Application.Models
{
    public class ProductDto
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string? CategoryName { get; set; }
    }
}
