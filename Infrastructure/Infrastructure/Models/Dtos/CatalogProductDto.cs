using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Models.Dtos;

public class CatalogProductDto
{
    [Required]
    public int Id { get; set; }
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = null!;
    [Required]
    [MaxLength(1000)]
    public string Description { get; set; } = null!;
    [Required]
    public decimal Price { get; set; }
    [Required]
    public int Amount { get; set; }
    [Required]
    public string PictureUrl { get; set; } = null!;
    [Required]
    [MaxLength(100)]
    public string Type { get; set; } = null!;
    [Required]
    [MaxLength(100)]
    public string Brand { get; set; } = null!;
}