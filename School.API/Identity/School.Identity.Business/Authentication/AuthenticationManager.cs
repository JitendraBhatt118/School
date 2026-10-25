using School.Entity.Identity;
using School.Identity.Business.JwtAuth;
using School.Identity.Data.Authentication;
using School.Infrastructure.Security;
using School.ViewModel.Common;
using School.ViewModel.Constants;
using School.ViewModel.Identity;
using System.Security.Claims;

namespace School.Identity.Business.Authentication;

public class AuthenticationManager
    (
        IAuthenticateRepository _authenticateRepository,
        IJwtAuthManager _jwtAuthManager
    ) : IAuthenticationManager
{
    public async Task<LoginResponseModel> AuthenticateUser(LoginModel request)
    {
        LoginResponseDataModel user
            = await _authenticateRepository.AuthenticateUser(request);

        if (user.UserId <= 0)
        {
            return Failure(AuthenticationStatus.InvalidEmail, IdentityMessages.InvalidEmail);
        }

        string requestPasswordHash = Hash.GenerateHash(request.Password);
        if (!string.Equals(user.Password, requestPasswordHash, StringComparison.Ordinal))
        {
            return Failure(AuthenticationStatus.InvalidPassword, IdentityMessages.InvalidPassword);
        }

        JwtAuthResponse authenticationResponse = await _jwtAuthManager.GenerateTokens(
        [
            new Claim(UserClaimModel.UserId, user.UserId.ToString()),
            new Claim(UserClaimModel.UserName, user.Email),
            new Claim(UserClaimModel.UserFullName, $"{user.FirstName} {user.LastName}".Trim()),
            new Claim(UserClaimModel.UserRole, user.RoleName)
        ], DateTime.UtcNow);

        return new LoginResponseModel
        {
            DataProcessResponse = new DataProcessResponseModel
            {
                IsSuccess = true,
                Message = Universal.ApiGeneralSuccessMessage,
                MessageKey = nameof(AuthenticationStatus.Success),
                RowId = user.UserId
            },
            AuthenticationResponse = authenticationResponse
        };
    }

    private static LoginResponseModel Failure(AuthenticationStatus status, string message)
    {
        return new LoginResponseModel
        {
            DataProcessResponse = new DataProcessResponseModel
            {
                IsSuccess = false,
                Message = message,
                MessageKey = status.ToString()
            }
        };
    }
}
