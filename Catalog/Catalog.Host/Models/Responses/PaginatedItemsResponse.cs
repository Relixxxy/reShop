namespace Catalog.Host.Models.Responses;

public class PaginatedItemsResponse<T>
    where T : class
{
    public int PageIndex { get; init; }

    public int PageSize { get; init; }

    public long Count { get; init; }

    public IEnumerable<T> Data { get; init; } = null!;
}
