using School.ViewModel.Common;

namespace School.ViewModel.Identity;

/// <summary>
/// Represents the login response model
/// </summary>
public class LoginResponseModel
{
    public DataProcessResponseModel DataProcessResponse { get; set; } = new();
    public JwtAuthResponse? AuthenticationResponse { get; set; } = new();
}
