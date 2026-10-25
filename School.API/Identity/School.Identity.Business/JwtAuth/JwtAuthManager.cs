using Microsoft.IdentityModel.Tokens;
using School.ViewModel.Common;
using School.ViewModel.Constants;
using School.ViewModel.Identity;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace School.Identity.Business.JwtAuth;

public class JwtAuthManager
    (
        JwtConfigurationsModel _jwtConfigurationsModel
    ) : IJwtAuthManager
{
    private readonly byte[] _secret
        = Encoding.ASCII.GetBytes(_jwtConfigurationsModel?.Secret ?? string.Empty);

    public Task<JwtAuthResponse> GenerateTokens(Claim[] claims, DateTime utcDateTime)
    {
        DateTime tokenExpiration
            = utcDateTime.AddMinutes(_jwtConfigurationsModel.AccessTokenExpirationMinutes);
        DateTime refreshTokenExpiration
            = utcDateTime.AddMinutes(_jwtConfigurationsModel.RefreshTokenExpirationMinutes);
        string refreshToken = GenerateRefreshTokenString();

        Claim[] claimsForToken = claims
            .Where(c => c.Type != UserClaimModel.RefreshToken
                        && c.Type != UserClaimModel.RefreshTokenExpireTime)
            .Concat(
            [
                new Claim(UserClaimModel.RefreshTokenExpireTime,
                    refreshTokenExpiration.ToString(
                        IdentityMessages.DateTimeFormatForTokenDates,
                        CultureInfo.InvariantCulture)),
                new Claim(UserClaimModel.RefreshToken, refreshToken)
            ])
            .ToArray();

        JwtSecurityTokenHandler jwtTokenHandler = new();
        SecurityTokenDescriptor tokenDescriptor = new()
        {
            Audience = _jwtConfigurationsModel.Audience,
            IssuedAt = utcDateTime,
            Issuer = _jwtConfigurationsModel.Issuer,
            Subject = new ClaimsIdentity(claimsForToken),
            Expires = tokenExpiration,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(_secret),
                SecurityAlgorithms.HmacSha256Signature)
        };

        SecurityToken token = jwtTokenHandler.CreateToken(tokenDescriptor);
        string accessToken = jwtTokenHandler.WriteToken(token);

        return Task.FromResult(new JwtAuthResponse
        {
            AccessToken = accessToken,
            AccessTokenExpireDateTime = tokenExpiration,
            RefreshToken = refreshToken,
            RefreshTokenExpireDateTime = refreshTokenExpiration
        });
    }

    private static string GenerateRefreshTokenString()
    {
        byte[] randomNumber = new byte[32];
        using RandomNumberGenerator randomNumberGenerator = RandomNumberGenerator.Create();
        randomNumberGenerator.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
}
