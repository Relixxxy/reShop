using MVC.ViewModels.Dtos;

namespace MVC.ViewModels;

public class Order
{
    public int Id { get; set; }
    public int OrderNumber { get; set; }
    public decimal TotalPrice { get; set; }
    public DateTime CreatedAt { get; set; }
    public IEnumerable<ProductDto> Products { get; set; } = null!;
}
