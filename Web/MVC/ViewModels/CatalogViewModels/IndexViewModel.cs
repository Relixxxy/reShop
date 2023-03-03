using MVC.ViewModels.Pagination;

namespace MVC.ViewModels.CatalogViewModels;

public class IndexViewModel
{
    public IEnumerable<ProductVM> Products { get; set; } = null!;
    public IEnumerable<SelectListItem> Brands { get; set; } = null!;
    public IEnumerable<SelectListItem> Types { get; set; } = null!;
    public int? BrandFilterApplied { get; set; }
    public int? TypesFilterApplied { get; set; }
    public PaginationInfo PaginationInfo { get; set; } = null!;
}
