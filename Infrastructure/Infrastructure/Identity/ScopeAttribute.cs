namespace Infrastructure.Identity;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class ScopeAttribute : Attribute
{
    public ScopeAttribute(string scopeName)
    {
        ScopeName = scopeName;
    }

    public string ScopeName { get; }
}
