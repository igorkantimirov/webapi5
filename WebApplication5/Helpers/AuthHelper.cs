using System.Security.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication5.Helpers;

/// <inheritdoc />
public class AuthHelper : IAuthHelper
{
    /// <inheritdoc />
    public string GetUserId(ControllerBase controller)
    {
        if (controller == null) 
            throw new ArgumentNullException(nameof(controller));
        
        var identity = controller.HttpContext.User.Identity as ClaimsIdentity;

        if (identity == null) 
            throw new AuthenticationException("No identity!");
        
        var nameClaim = identity.FindFirst(ClaimTypes.Name);
        if (nameClaim == null) 
            throw new ApplicationException("Cannot get claim 'Name'.");
        
        return nameClaim.Value;
    }
}