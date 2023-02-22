using System.ComponentModel.DataAnnotations;

namespace Catalog.Host.Models.Requests;

public class CreateProductRequest
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = null!;
    [Required]
    [MaxLength(1000)]
    public string Description { get; set; } = null!;
    [Required]
    public decimal Price { get; set; }
    [Required]
    [MaxLength(100)]
    public string PictureFileName { get; set; } = null!;
    [Required]
    [MaxLength(100)]
    public string CatalogType { get; set; } = null!;
    [Required]
    [MaxLength(100)]
    public string CatalogBrand { get; set; } = null!;
    [Required]
    public int AvailableStock { get; set; }
}