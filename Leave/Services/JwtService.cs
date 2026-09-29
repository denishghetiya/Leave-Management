//using Microsoft.Extensions.Configuration;
//using Microsoft.IdentityModel.Tokens;
//using System;
//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;
//using System.Text;

//namespace YourNamespace.Services
//{
//    public class JwtService
//    {
//        private readonly string _secretKey;
//        private readonly string _issuer;
//        private readonly string _audience;
//        private readonly int _expiryMinutes;

//        public JwtService(IConfiguration configuration)
//        {
//            _secretKey = configuration["JwtSettings:SecretKey"];
//            _issuer = configuration["JwtSettings:Issuer"];
//            _audience = configuration["JwtSettings:Audience"];
//            _expiryMinutes = int.Parse(configuration["JwtSettings:ExpiryMinutes"]);
//        }

//        public string GenerateToken(string username, string role)
//        {
//            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
//            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

//            var claims = new[]
//            {
//                new Claim(ClaimTypes.Name, username),
//                new Claim(ClaimTypes.Role, role),
//                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) // Unique identifier
//            };

//            var token = new JwtSecurityToken(
//                issuer: _issuer,
//                audience: _audience,
//                claims: claims,
//                expires: DateTime.UtcNow.AddMinutes(_expiryMinutes),
//                signingCredentials: credentials
//            );

//            return new JwtSecurityTokenHandler().WriteToken(token);
//        }
//    }
//}



//using System;
//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;
//using System.Text;
//using Microsoft.Extensions.Configuration;
//using Microsoft.IdentityModel.Tokens;
//using Leave.DBContext;

//public class JwtService
//{
//    private readonly string _key;
//    private readonly string _issuer;
//    private readonly string _audience;

//    public JwtService(IConfiguration config)
//    {
//        _key = config["Jwt:Key"];
//        _issuer = config["Jwt:Issuer"];
//        _audience = config["Jwt:Audience"];
//    }

//    public string GenerateToken(User user)
//    {
//        var claims = new[]
//        {
//            new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
//            new Claim(ClaimTypes.Name, user.Username),
//            new Claim(ClaimTypes.Role, user.IsAdmin ? "Admin" : "User")
//        };

//        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));
//        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

//        var token = new JwtSecurityToken(_issuer, _audience, claims, expires: DateTime.UtcNow.AddHours(1), signingCredentials: creds);

//        return new JwtSecurityTokenHandler().WriteToken(token);
//    }
//}



//using Microsoft.IdentityModel.Tokens;
//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;
//using System.Text;
//using Microsoft.Extensions.Configuration;

//namespace Leave.Services
//{
//    public class JwtService
//    {
//        private readonly IConfiguration _config;

//        public JwtService(IConfiguration config)
//        {
//            _config = config;
//        }

//        public string GenerateToken(int userId, string username, bool isAdmin)
//        {
//            var jwtSettings = _config.GetSection("JwtSettings");
//            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]));

//            var claims = new List<Claim>
//            {
//                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
//                new Claim(JwtRegisteredClaimNames.UniqueName, username),
//                new Claim(ClaimTypes.Role, isAdmin ? "Admin" : "User")
//            };

//            var token = new JwtSecurityToken(
//                issuer: jwtSettings["Issuer"],
//                audience: jwtSettings["Audience"],
//                claims: claims,
//                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings["ExpiryInMinutes"])),
//                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
//            );

//            return new JwtSecurityTokenHandler().WriteToken(token);
//        }
//    }
//}
