namespace MVC.ViewModels;

public record Header
{
    public string Controller { get; init; } = null!;
    public string Text { get; init; } = null!;
}
