namespace School.ViewModel.Identity;

/// <summary>
/// Represents jwt auth response model
/// </summary>
public class JwtAuthResponse
{
    public string? AccessToken { get; set; }
    public DateTime? AccessTokenExpireDateTime { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpireDateTime { get; set; }
}
