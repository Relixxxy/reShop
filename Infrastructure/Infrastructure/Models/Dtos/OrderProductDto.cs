using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Models.Dtos;

public class OrderProductDto
{
    [Required]
    public int Id { get; set; }
    [Required]
    public int ProductId { get; set; }
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = null!;
    [Required]
    public decimal Price { get; set; }
    [Required]
    public int Amount { get; set; }
}
