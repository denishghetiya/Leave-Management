//using Leave.DBContext;
//using Leave.ViewModel;
//using Leave.ViewModels;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.IdentityModel.Tokens;
//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;
//using System.Text;
//using BCrypt.Net;


//namespace Leave.Controllers 
//{
//    [Route("api/auth")]
//    [ApiController]
//    public class AuthController : ControllerBase
//    {
//        private readonly ApplicationDBContext _context;
//        private readonly IConfiguration _config;

//        public AuthController(ApplicationDBContext context, IConfiguration config)
//        {
//            _context = context;
//            _config = config;
//        }

//        [HttpPost("login")]
//        public IActionResult Login([FromBody] LoginViewModel model)
//        {
//            var user = _context.Users.FirstOrDefault(u => u.Email == model.Email);

//            if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.Password))
//            {
//                return Unauthorized(new { message = "Invalid email or password" });
//            }

//            if (!user.IsActive)
//            {
//                return Unauthorized(new { message = "User is not active. Contact Admin." });
//            }

//            var token = GenerateJwtToken(user);
//            return Ok(new { token });
//        }

//        private string GenerateJwtToken(User user)
//        {
//            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtSettings:Secret"]));
//            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

//            var claims = new[]
//            {
//                new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
//                new Claim(JwtRegisteredClaimNames.Email, user.Email),
//                new Claim(ClaimTypes.Role, user.IsAdmin ? "Admin" : "User")
//            };

//            var token = new JwtSecurityToken(
//                issuer: _config["Jwt:Issuer"],
//                audience: _config["Jwt:Audience"],
//                claims: claims,
//                expires: DateTime.UtcNow.AddHours(2),
//                signingCredentials: credentials
//            );

//            return new JwtSecurityTokenHandler().WriteToken(token);
//        }
//    }
//}
