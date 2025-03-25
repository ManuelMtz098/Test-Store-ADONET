using System.Text.Json.Serialization;

namespace Test_Store_ADONET.Entities
{
    public class UserLogin
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        [JsonIgnore]
        public string Password { get; set; }
    }
}
