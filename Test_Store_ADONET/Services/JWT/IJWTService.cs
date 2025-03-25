using Test_Store_ADONET.Entities;

namespace Test_Store_ADONET.Services.JWT
{
    public interface IJWTService
    {
        string GenerateAccessToken(string username);
    }
}