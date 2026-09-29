using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Leave.DBContext;
using Leave.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Leave.Services;
using ClosedXML.Excel;


namespace Leave.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDBContext _context;
        private readonly EmailService _emailService;
        public AdminController(ApplicationDBContext context, EmailService emailService)
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
        public IActionResult CreateUser()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    Username = model.Username,
                    Email = model.Email,
                    Password = model.Password, 
                    IsAdmin = model.IsAdmin,
                    IsActive = model.IsActive
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return RedirectToAction("UserList");
            }

            return View(model);
        }

        public async Task<IActionResult> UserList()
        {
            var users = await _context.Users.ToListAsync();
            return View(users);
        }

        public async Task<IActionResult> AllLeaveList()
        {
            var leaves = await _context.AllLeaves.Include(l => l.User).ToListAsync();
            return View(leaves);
        }

        [HttpPost]
        public async Task<IActionResult> ApproveRejectLeave(int leaveId, string isApprove)
        {
            ViewBag.Username = User.Identity.IsAuthenticated
                ? User.Identity.Name
                : "Guest";

            var leave = await _context.AllLeaves.FindAsync(leaveId);

            if (leave != null)
            {
                if (!string.IsNullOrEmpty(leave.IsApprove))
                {
                    return RedirectToAction("AllLeaveList");
                }

                leave.IsApprove = isApprove;
                await _context.SaveChangesAsync();
            }
            var user = await _context.Users.FindAsync(leave.UserId);
            if (user != null)
            {
                string subject = "Leave Request Update";
                string body = $@"<!DOCTYPE html>
                    <html>
                    <head>
                        <title>Leave Request Status</title>
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
                            .status {{
                                text-align: center;
                                font-weight: bold;
                                padding: 10px;
                                border-radius: 4px;
                            }}
                            .approved {{
                                background-color: #d4edda;
                                color: #155724;
                            }}
                            .rejected {{
                                background-color: #f8d7da;
                                color: #721c24;
                            }}
                            .details-table {{
                                width: 100%;
                                border-collapse: collapse;
                                margin-top: 15px;
                            }}
                            .details-table th, .details-table td {{
                                border: 1px solid #ddd;
                                padding: 10px;
                                text-align: left;
                            }}
                            .details-table th {{
                                background-color: #f0f0f0;
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
                        </style>
                    </head>
                    <body>
                        <div class=""container"">
                            <div class=""header"">
                                <h2>Leave Request Status</h2>
                            </div>
                            <div class=""content"">
                                <p>Dear {user.Username},</p>
                                
                                <!-- Status Message -->
                                <div class=""status approved"">
                                    Your leave request has been <b>{isApprove}</b>.
                                </div>
                                
                                <!-- Uncomment for Rejection -->
                                <!-- <div class=""status rejected"">
                                    Your leave request has been <b>REJECTED</b>.
                                </div> -->
                                
                                <p>Here are the details of your leave request:</p>
                    
                                <table class=""details-table"">
                                    <tr>
                                        <th>User ID</th>
                                        <td>{user.UserId}</td>
                                    </tr>
                                    <tr>
                                        <th>Username</th>
                                        <td>{user.Username}</td>
                                    </tr>
                                    <tr>
                                        <th>Start Date</th>
                                        <td>{leave.FromDate:dd-MM-yyyy}</td>
                                    </tr>
                                    <tr>
                                        <th>End Date</th>
                                        <td>{leave.ToDate:dd-MM-yyyy}</td>
                                    </tr>
                                    <tr>
                                        <th>Reason</th>
                                        <td>{leave.Reason}</td>
                                    </tr>
                                    <tr>
                                        <th>Approved By</th>
                                        <td>{ViewBag.Username}</td>
                                    </tr>
                                    
                                </table>
                    
                                <p>If you have any questions, feel free to contact HR.</p>
                    
                                <p>Best Regards,<br>
                                Denish Technology<br></p>
                            </div>
                            <div class=""footer"">
                                © [2025] [Denish Technology] - All Rights Reserved
                            </div>
                        </div>
                    </body>
                    </html>";

                await _emailService.SendEmailAsync(user.Email, subject, body);
            }

            return RedirectToAction("AllLeaveList");
        }

        public async Task<IActionResult> ToggleUserStatus(int userId)
        {
            var user = await _context.Users.FindAsync(userId);

            if (user != null)
            {
                user.IsActive = !user.IsActive; 
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("UserList"); 
        }
        public async Task<IActionResult> EditUser(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }
        [HttpPost]
        public async Task<IActionResult> EditUser(User user)
        {
            var existingUser = await _context.Users.FindAsync(user.UserId);
            if (existingUser == null)
            {
                return NotFound();
            }

            existingUser.Username = user.Username;
            existingUser.Email = user.Email;
            existingUser.IsAdmin = user.IsAdmin;
            existingUser.IsActive = user.IsActive;

            _context.Entry(existingUser).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return RedirectToAction("UserList");
        }

        [HttpGet]
        public async Task<IActionResult> DownloadLeaveList()
        {
            var leaves = await _context.AllLeaves
                .Select(l => new
                {
                    l.UserId,
                    l.Reason,
                    l.FromDate,
                    l.ToDate,
                    l.IsApprove
                })
                .ToListAsync();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Leave List");
                var currentRow = 1;

                // Header
                worksheet.Cell(currentRow, 1).Value = "User ID";
                worksheet.Cell(currentRow, 2).Value = "Reason";
                worksheet.Cell(currentRow, 3).Value = "From Date";
                worksheet.Cell(currentRow, 4).Value = "To Date";
                worksheet.Cell(currentRow, 5).Value = "IsApprove";

                // Data
                foreach (var leave in leaves)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = leave.UserId;
                    worksheet.Cell(currentRow, 2).Value = leave.Reason;
                    worksheet.Cell(currentRow, 3).Value = leave.FromDate.ToString("yyyy-MM-dd");
                    worksheet.Cell(currentRow, 4).Value = leave.ToDate.HasValue
                        ? leave.ToDate.Value.ToString("yyyy-MM-dd")  
                        : "";  
                    worksheet.Cell(currentRow, 5).Value = leave.IsApprove;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "LeaveList.xlsx");
                }
            }
        }

    }
}



