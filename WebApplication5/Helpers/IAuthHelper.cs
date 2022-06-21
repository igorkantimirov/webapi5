using Microsoft.AspNetCore.Mvc;

namespace WebApplication5.Helpers;

public interface IAuthHelper
{
    /// <summary>
    /// Gets the identifier of the user that made current request.
    /// It will use the first "Name" claim as identifier and cast it to GUID.
    /// </summary>
    /// <param name="controller">The controller.</param>
    /// <returns>The identifier of the user that made current request.</returns>
    string GetUserId(ControllerBase controller);
}