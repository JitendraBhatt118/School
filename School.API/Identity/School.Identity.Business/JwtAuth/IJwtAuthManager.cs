using School.ViewModel.Identity;
using System.Security.Claims;

namespace School.Identity.Business.JwtAuth;

public interface IJwtAuthManager
{
    Task<JwtAuthResponse> GenerateTokens(Claim[] claims, DateTime utcDateTime);
}
