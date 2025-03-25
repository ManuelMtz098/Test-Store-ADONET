using System.Data;
using Test_Store_ADONET.Entities;

namespace Test_Store_ADONET.Mappers
{
    public static class LoginMapper
    {
        public static UserLogin MapToUserLogin(DataRow dr)
        {
            return new UserLogin
            {
                Firstname = dr.Field<string>("FirstName"),
                Lastname = dr.Field<string>("LastName"),
                Password = dr.Field<string>("Password")
            };
        }
    }
}
