using Test_Store_ADONET.DTOs;
using Test_Store_ADONET.Entities;

namespace Test_Store_ADONET.Services.Login
{
    public interface ILoginService
    {
        Task<UserLoginWithToken> Login(LoginDTO login);
    }
}