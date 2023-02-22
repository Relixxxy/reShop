using System.ComponentModel.DataAnnotations;

namespace Catalog.Host.Models.Requests;

public class IdRequest
{
    [Required]
    public int Id { get; set; }
}