namespace School.ViewModel.Common;

/// <summary>
/// Represents jwt configurations model
/// </summary>
public class JwtConfigurationsModel
{
    public string? Issuer { get; set; }
    public string? Audience { get; set; }
    public string? Secret { get; set; }
    public int AccessTokenExpirationMinutes { get; set; }
    public int RefreshTokenExpirationMinutes { get; set; }
}
