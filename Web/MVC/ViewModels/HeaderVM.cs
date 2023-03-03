namespace MVC.ViewModels;

public record HeaderVM
{
    public string Controller { get; init; } = null!;
    public string Text { get; init; } = null!;
}
