using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Models.Requests;

public class IdRequest
{
    [Required]
    public int Id { get; set; }
}