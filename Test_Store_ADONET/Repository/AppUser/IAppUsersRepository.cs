using System.Data;

namespace Test_Store_ADONET.Repository.AppUser
{
    public interface IAppUsersRepository
    {
        Task<DataRow> GetUserByUsername(string username);
    }
}
