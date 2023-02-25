namespace MVC.ViewModels;

public record ProductsCatalog
{
    public int PageIndex { get; init; }
    public int PageSize { get; init; }
    public int Count { get; init; }
    public List<Product> Data { get; init; } = null!;
}
