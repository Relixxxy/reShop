namespace Catalog.Host.Models.Requests;

public class CreateProductRequest
{
    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public decimal Price { get; set; }

    public string PictureFileName { get; set; } = null!;

    public string CatalogType { get; set; } = null!;

    public string CatalogBrand { get; set; } = null!;

    public int AvailableStock { get; set; }
}