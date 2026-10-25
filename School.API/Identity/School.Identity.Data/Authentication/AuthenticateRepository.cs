using School.Entity.Identity;
using School.Infrastructure.Persistence.Dapper;
using School.ViewModel.Constants;
using School.ViewModel.Identity;

namespace School.Identity.Data.Authentication;

public class AuthenticateRepository(IDapperExecutor _dapperExecutor)
    : IAuthenticateRepository
{
    public async Task<LoginResponseDataModel> AuthenticateUser(LoginModel request)
    {
        return await _dapperExecutor.QuerySingleAsync<LoginResponseDataModel>(
            StoredProcedures.AuthenticateUser,
            new { Email = request.Username })
            ?? new LoginResponseDataModel();
    }
}
