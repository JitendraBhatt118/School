using School.ViewModel.Identity;

namespace School.Identity.Business.Authentication;

public interface IAuthenticationManager
{
    Task<LoginResponseModel> AuthenticateUser(LoginModel request);
}
