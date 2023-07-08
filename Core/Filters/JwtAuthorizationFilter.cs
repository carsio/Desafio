
using Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Core.Filters;

public class JwtAuthorizationFilter : IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var authService = context.HttpContext.RequestServices.GetRequiredService<IAuthService>();
        var authSucceeded = authService.Authenticate(context.HttpContext);

        if (!authSucceeded)
            context.Result = new UnauthorizedResult();
    }
}
