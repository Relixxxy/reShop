using MVC.Services.Interfaces;
using MVC.ViewModels;

namespace MVC.Services;

public class IdentityParser : IIdentityParser<ApplicationUserVM>
{
    public ApplicationUserVM Parse(IPrincipal principal)
    {
        // Pattern matching 'is' expression
        // assigns "claims" if "principal" is a "ClaimsPrincipal"
        if (principal is ClaimsPrincipal claims)
        {
            return new ApplicationUserVM
            {
                CardHolderName = claims.Claims.FirstOrDefault(x => x.Type == "card_holder")?.Value ?? string.Empty,
                CardNumber = claims.Claims.FirstOrDefault(x => x.Type == "card_number")?.Value ?? string.Empty,
                Expiration = claims.Claims.FirstOrDefault(x => x.Type == "card_expiration")?.Value ?? string.Empty,
                CardType = int.Parse(claims.Claims.FirstOrDefault(x => x.Type == "missing")?.Value ?? "0"),
                City = claims.Claims.FirstOrDefault(x => x.Type == "address_city")?.Value ?? string.Empty,
                Country = claims.Claims.FirstOrDefault(x => x.Type == "address_country")?.Value ?? string.Empty,
                Email = claims.Claims.FirstOrDefault(x => x.Type == "email")?.Value ?? string.Empty,
                Id = claims.Claims.FirstOrDefault(x => x.Type == "sub")?.Value ?? string.Empty,
                LastName = claims.Claims.FirstOrDefault(x => x.Type == "last_name")?.Value ?? string.Empty,
                Name = claims.Claims.FirstOrDefault(x => x.Type == "name")?.Value ?? string.Empty,
                PhoneNumber = claims.Claims.FirstOrDefault(x => x.Type == "phone_number")?.Value ?? string.Empty,
                SecurityNumber = claims.Claims.FirstOrDefault(x => x.Type == "card_security_number")?.Value ?? string.Empty,
                State = claims.Claims.FirstOrDefault(x => x.Type == "address_state")?.Value ?? string.Empty,
                Street = claims.Claims.FirstOrDefault(x => x.Type == "address_street")?.Value ?? string.Empty,
                ZipCode = claims.Claims.FirstOrDefault(x => x.Type == "address_zip_code")?.Value ?? string.Empty
            };
        }

        throw new ArgumentException(message: "The principal must be a ClaimsPrincipal", paramName: nameof(principal));
    }
}