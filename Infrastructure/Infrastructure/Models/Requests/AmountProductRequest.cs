using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Models.Requests;

public class AmountProductRequest
{
    [Required]
    public int ProductId { get; set; }
    [Required]
    public int Amount { get; set; }
}
