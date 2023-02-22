namespace Catalog.Host.Models.Dtos;

public class ProductDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public decimal Price { get; set; }
    public int AvailableStock { get; set; }
    public string PictureUrl { get; set; } = null!;
    public string Type { get; set; } = null!;
    public string Brand { get; set; } = null!;
}
