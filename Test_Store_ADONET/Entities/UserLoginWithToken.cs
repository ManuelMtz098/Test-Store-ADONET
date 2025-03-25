namespace Test_Store_ADONET.Entities
{
    public class UserLoginWithToken:UserLogin
    {
        public UserLoginWithToken(UserLogin user)
        {
            Firstname = user.Firstname;
            Lastname = user.Lastname;
        }
        public string AccessToken { get; set; }
    }
}
