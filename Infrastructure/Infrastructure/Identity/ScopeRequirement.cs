using Microsoft.AspNetCore.Authorization;

namespace Infrastructure.Identity;

public class ScopeRequirement : IAuthorizationRequirement
{
    public ScopeRequirement()
    {
    }
}
