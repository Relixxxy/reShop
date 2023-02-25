namespace MVC.Dtos;

public class PaginatedItemsRequest<T>
    where T : notnull
{
    public int PageIndex { get; set; }

    public int PageSize { get; set; }
    public Dictionary<T, string>? Filters { get; set; } = null!;
}