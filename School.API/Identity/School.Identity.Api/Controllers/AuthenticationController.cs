using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using School.Identity.Business.Authentication;
using School.Infrastructure.Controller;
using School.ViewModel.Identity;

namespace School.Identity.Api.Controllers;

/// <summary>
/// Represents the authentication controller
/// </summary>
[Route("v1/authentication")]
[ApiController]
public class AuthenticationController
    (
        IAuthenticationManager _authenticationManager
    ) : BaseController
{
    /// <summary>
    /// Authenticate user
    /// </summary>
    /// <param name="login">LoginModel object</param>
    /// <returns>ApiResponse</returns>
    [AllowAnonymous]
    [HttpPost]
    [ProducesResponseType(typeof(LoginResponseModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> AuthenticateUser(LoginModel login)
    {
        return Success(await _authenticationManager.AuthenticateUser(login));
    }
}
