using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Models.Requests;

public class PaginatedItemsRequest<T>
    where T : notnull
{
    [Required]
    public int PageIndex { get; set; }
    [Required]
    public int PageSize { get; set; }
    public Dictionary<T, string>? Filters { get; set; } = null!;
}