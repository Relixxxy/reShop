namespace MVC.ViewModels;

public record ProductsCatalogVM
{
    public int PageIndex { get; init; }
    public int PageSize { get; init; }
    public int Count { get; init; }
    public List<ProductVM> Data { get; init; } = null!;
}
