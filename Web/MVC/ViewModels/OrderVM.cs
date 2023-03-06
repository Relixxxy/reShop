using Infrastructure.Models.Dtos;

namespace MVC.ViewModels;

public class OrderVM
{
    public int Id { get; set; }
    public string OrderNumber { get; set; } = null!;
    public decimal TotalPrice { get; set; }
    public DateTime CreatedAt { get; set; }
    public IEnumerable<OrderProductDto> Products { get; set; } = null!;
}
