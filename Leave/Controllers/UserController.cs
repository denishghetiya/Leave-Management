using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Leave.DBContext;
using Leave.ViewModels;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Leave.Services;

namespace Leave.Controllers
{
    [Authorize(Roles = "User")]
    //[Route("api/[controller]")]
    //[ApiController]
    public class UserController : Controller
    {
        private readonly ApplicationDBContext _context;
        private readonly EmailService _emailService;

        public UserController(ApplicationDBContext context, EmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }
        public IActionResult Dashboard()
        {
            ViewBag.Username = User.Identity.IsAuthenticated
                ? User.Identity.Name
                : "Guest";

            return View();
        }
        
        [HttpGet]
        public IActionResult CreateLeave()
        {
            ViewBag.Username = User.Identity.IsAuthenticated
                ? User.Identity.Name
                : "Guest";

            var userId = User.FindFirstValue("UserID");

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            ViewBag.UserID = userId;
            return View();
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]  // Prevents cross-site request forgery
        public async Task<IActionResult> CreateLeave(LeaveViewModel model)
        {
            if (ModelState.IsValid)
            {
                var leave = new AllLeaf
                {
                    UserId = model.UserId,
                    Reason = model.Reason,
                    FromDate = model.FromDate,
                    ToDate = model.ToDate
                };

                _context.AllLeaves.Add(leave);
                await _context.SaveChangesAsync();

                var adminEmail = model.Email;
                string subject = "New Leave Request";
                string body = $@"<!DOCTYPE html>
                    <html>
                    <head>
                        <title>Leave Request</title>
                        <style>
                            body {{
                                font-family: Arial, sans-serif;
                                background-color: #f3f3f3;
                                padding: 20px;
                                margin: 0;
                            }}
                            .container {{
                                max-width: 600px;
                                background-color: #ffffff;
                                margin: 0 auto;
                                border: 1px solid #ddd;
                                border-radius: 8px;
                                padding: 20px;
                                box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
                            }}
                            .header {{
                                text-align: center;
                                background-color: #4CAF50;
                                color: #ffffff;
                                padding: 10px 0;
                                border-top-left-radius: 8px;
                                border-top-right-radius: 8px;
                            }}
                            .content {{
                                padding: 20px;
                            }}
                            .footer {{
                                text-align: center;
                                padding: 10px;
                                background-color: #f3f3f3;
                                color: #777;
                                border-bottom-left-radius: 8px;
                                border-bottom-right-radius: 8px;
                                font-size: 12px;
                            }}
                            .btn {{
                                display: inline-block;
                                background-color: #4CAF50;
                                color: #ffffff;
                                text-decoration: none;
                                padding: 10px 20px;
                                border-radius: 4px;
                                margin-top: 20px;
                            }}
                            .details-table {{
                                width: 100%;
                                border-collapse: collapse;
                            }}
                            .details-table th, .details-table td {{
                                border: 1px solid #ddd;
                                padding: 10px;
                                text-align: left;
                            }}
                            .details-table th {{
                                background-color: #f0f0f0;
                            }}
                        </style>
                    </head>
                    <body>
                        <div class=""container"">
                            <div class=""header"">
                                <h2>Leave Request Form</h2>
                            </div>
                            <div class=""content"">
                                <p>Respected Sir,</p>
                                <p>I would like to formally request leave for the following period:</p>
                                
                                <table class=""details-table"">
                                    <tr>
                                        <th>Employee Name</th>
                                        <td>{model.Username}</td>
                                    </tr>
                                    <tr>
                                        <th>UserID</th>
                                        <td>{model.UserId}</td>
                                    </tr>
                                    
                                    <tr>
                                        <th>Start Date</th>
                                        <td>{model.FromDate:yyyy-MM-dd}</td>
                                    </tr>
                                    <tr>
                                        <th>End Date</th>
                                        <td>{model.ToDate:yyyy-MM-dd}</td>
                                    </tr>
                                    <tr>
                                        <th>Reason</th>
                                        <td>{model.Reason}</td>
                                    </tr>
                                </table>
                    
                                <p>If you require any additional information, please let me know.</p>
                    
                                <p>Best Regards,<br>
                                {model.Username}<br></p>
                    
                                <a href=""https://localhost:44376/Admin/AllLeaveList"" class=""btn"">Approve/Reject</a>
                            </div>
                            <div class=""footer"">
                                © [2025] [Krista Technology] - All Rights Reserved
                            </div>
                        </div>
                    </body>
                    </html>";

                await _emailService.SendEmailAsync(adminEmail, subject, body);

                return RedirectToAction("UserLeaveList");
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> UserLeaveList()
        {
            var userId = User.FindFirstValue("UserID");

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            // Convert userId to int (assuming UserId in DB is an integer)
            if (!int.TryParse(userId, out int parsedUserId))
            {
                return RedirectToAction("Login", "Account");
            }

            var leaves = await _context.AllLeaves
                .Where(l => l.UserId == parsedUserId)  // Ensure correct data type
                .ToListAsync();

            return View(leaves);
        }

        //[HttpGet("get-users")]
        ////[Authorize] // Protects this endpoint with JWT authentication
        //public IActionResult GetUsers()
        //{
        //    return Ok(new { Message = "This is a protected route for users" });
        //}

        //[HttpPost("create-user")]
        ////[Authorize(Roles = "Admin")] // Only Admin can create users
        //public IActionResult CreateUser()
        //{
        //    return Ok(new { Message = "User created successfully" });
        //}

        //[HttpGet("profile")]
        //public IActionResult GetProfile()
        //{
        //    var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        //    return Ok(new { Message = "User is authenticated", UserID = userId });
        //}
    }
}
