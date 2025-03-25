using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace Test_Store_ADONET.Services.JWT
{
    public class JWTService : IJWTService
    {
        private readonly JWTSettings _jwtSettings;

        public JWTService(IOptions<JWTSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
        }

        public string GenerateAccessToken(string username)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _jwtSettings.Issuer,  // Emisor del token
                _jwtSettings.Audience, // Audiencia para el que está destinado
                claims,                // Los claims del token
                expires: DateTime.Now.AddMinutes(10),  // Tiempo de expiración del token
                signingCredentials: credentials  // Las credenciales de firma
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
