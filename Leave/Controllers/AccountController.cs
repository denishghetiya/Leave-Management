using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Linq;
using Leave.DBContext;
using Leave.ViewModel;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Leave.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDBContext _context;

        public AccountController(ApplicationDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var user = _context.Users
                .FirstOrDefault(u => u.Email == model.Email && u.Password == model.Password);

            if (user != null)
            {
                if (!user.IsActive)
                {
                    ModelState.AddModelError("", "Your account is deactivated. Please contact admin.");
                    return View(model);
                }

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.IsAdmin ? "Admin" : "User"),
                    new Claim("UserID", user.UserId.ToString())
                };

                var identity = new ClaimsIdentity(claims, "Cookies");
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync("Cookies", principal);

                return (IActionResult)(user.IsAdmin
                        ? RedirectToAction("Dashboard", "Admin")
                        : RedirectToAction("Dashboard", "User"));

            }

            ModelState.AddModelError("", "Invalid Email or Password.");
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("Cookies");
            return RedirectToAction("Login");
        }

        public IActionResult AccessDenied() => View();
    }
}



//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Http;
//using System.Linq;
//using System.Security.Claims;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using Leave.DBContext;
//using Leave.ViewModel;
//using Leave.Services;
//using Microsoft.AspNetCore.Authentication.Cookies;
//using Microsoft.AspNetCore.Authentication;
//using Microsoft.AspNetCore.Authorization;
//using Leave.Services;

//namespace Leave.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class AccountController : ControllerBase
//    {
//        private readonly ApplicationDBContext _context;
//        //private readonly JwtService _jwtService;

//        public AccountController(ApplicationDBContext context)
//        {
//            _context = context;
//            _jwtService = jwtService;
//        }

//        [HttpPost("login")]
//        public IActionResult Login([FromBody] LoginViewModel model)
//        {
//            var user = _context.Users
//                .FirstOrDefault(u => u.Email == model.Email && u.Password == model.Password);

//            if (user != null)
//            {
//                if (!user.IsActive)
//                {
//                    return Unauthorized(new { Message = "Your account is deactivated. Please contact admin." });
//                }

//                var token = _jwtService.GenerateToken(user.Username, user.IsAdmin ? "Admin" : "User");
//                return Ok(new { Token = token });
//            }

//            return Unauthorized(new { Message = "Invalid Email or Password." });
//        }

//        [HttpPost("logout")]
//        public async Task<IActionResult> Logout()
//        {
//            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
//            return Ok(new { Message = "Logged out successfully" });
//        }

//        [HttpGet("access-denied")]
//        public IActionResult AccessDenied()
//        {
//            return Forbid();
//        }
//    }
//}