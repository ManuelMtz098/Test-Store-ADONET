using System.Data;
using Test_Store_ADONET.DTOs;
using Test_Store_ADONET.Entities;
using Test_Store_ADONET.Exceptions;
using Test_Store_ADONET.Mappers;
using Test_Store_ADONET.Repository.AppUser;
using Test_Store_ADONET.Services.JWT;

namespace Test_Store_ADONET.Services.Login
{
    public class LoginService : ILoginService
    {
        IAppUsersRepository _appUsersRepository;
        private readonly IJWTService _jwt;
        public LoginService(IAppUsersRepository appUsersRepository, IJWTService jwt)
        {
            _appUsersRepository = appUsersRepository;
            _jwt = jwt;
        }

        public async Task<UserLoginWithToken> Login(LoginDTO login)
        {
            DataRow drUser = await _appUsersRepository.GetUserByUsername(login.Username);

            if (drUser == null)
                throw new NotFoundException("User not found.");

            UserLogin userLogin = LoginMapper.MapToUserLogin(drUser);

            bool isValid = BCrypt.Net.BCrypt.Verify(login.Password, userLogin.Password);

            if(!isValid)
                throw new BadRequestException("Invalid password.");

            string accessToken = _jwt.GenerateAccessToken(login.Username);

            return new UserLoginWithToken(userLogin)
            {
                AccessToken = accessToken
            };
        }
    }
}
