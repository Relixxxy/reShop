namespace Infrastructure.Models.Dtos;

public class OrderDto
{
    public int Id { get; set; }
    public int OrderNumber { get; set; }
    public decimal TotalPrice { get; set; }
    public DateTime CreatedAt { get; set; }
    public IEnumerable<OrderProductDto> Products { get; set; } = null!;
}
