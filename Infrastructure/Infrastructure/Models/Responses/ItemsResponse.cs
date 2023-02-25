namespace Infrastructure.Models.Responses;

public class ItemsResponse<T>
    where T : class
{
    public IEnumerable<T> Items { get; set; } = null!;
}
