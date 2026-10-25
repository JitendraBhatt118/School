using School.Entity.Identity;
using School.ViewModel.Identity;

namespace School.Identity.Data.Authentication;

public interface IAuthenticateRepository
{
    Task<LoginResponseDataModel> AuthenticateUser(LoginModel request);
}
