namespace Order.Data.Entities;

public class OrderEntity
{
    public int Id { get; set; }
    public string UserId { get; set; } = null!;
    public string OrderNumber { get; set; } = null!;
    public decimal TotalPrice { get; set; }
    public DateTime CreatedAt { get; set; }
    public IEnumerable<ProductEntity> Products { get; set; } = null!;
}
