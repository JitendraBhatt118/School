namespace School.Entity.Identity;

/// <summary>
/// Stored procedure result for usp_AuthenticateUser.
/// </summary>
public class LoginResponseDataModel
{
    public long UserId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public int RoleId { get; set; }
    public string RoleName { get; set; } = string.Empty;
}
