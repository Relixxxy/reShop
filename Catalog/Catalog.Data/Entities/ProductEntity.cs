namespace Catalog.Data.Entities;

public class ProductEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public decimal Price { get; set; }
    public int AvailableStock { get; set; }
    public string PictureFileName { get; set; } = null!;
    public string Type { get; set; } = null!;
    public string Brand { get; set; } = null!;
}
