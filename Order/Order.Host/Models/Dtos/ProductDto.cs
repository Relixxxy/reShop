namespace Order.Host.Models.Dtos;

public class ProductDto
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
    public int Amount { get; set; }
}
