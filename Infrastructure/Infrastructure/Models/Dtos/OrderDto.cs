using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Models.Dtos;

public class OrderDto
{
    [Required]
    public int Id { get; set; }
    [Required]
    [MaxLength(100)]
    public string OrderNumber { get; set; } = null!;
    [Required]
    public decimal TotalPrice { get; set; }
    [Required]
    public DateTime CreatedAt { get; set; }
    public IEnumerable<OrderProductDto> Products { get; set; } = null!;
}
