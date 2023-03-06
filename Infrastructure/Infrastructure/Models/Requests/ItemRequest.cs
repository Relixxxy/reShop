using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Models.Requests;

public class ItemRequest<T>
    where T : class
{
    [Required]
    public T Item { get; set; } = null!;
}
