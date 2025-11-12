namespace Application.Models.Response
{
    public class OrderResponseDTO
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public int ClientId { get; set; }

        public int Quantity { get; set; }
        public DateTime Date { get; set; }
        public string? ProductName { get; set; }
        public string? ClientName { get; set; }
        public decimal Total { get; set; }
    }
}