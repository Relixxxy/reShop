namespace MVC.Services.Interfaces;

public interface IIdentityParser<T>
{
    T Parse(IPrincipal principal);
}