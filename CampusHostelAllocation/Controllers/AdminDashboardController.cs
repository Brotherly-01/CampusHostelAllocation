using CampusHostelAllocation.Data;
using CampusHostelAllocation.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CampusHostelAllocation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AdminDashboardController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AdminDashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetDashboard()
        {
            var dashboard = new AdminDashboardDTO
            {
                TotalStudents = await _context.Users.CountAsync(
                    u => u.UserName != "admin@campushostel.com"),

                ActiveStudents = await _context.Users.CountAsync(
                    u => u.UserName != "admin@campushostel.com"
                    && u.IsActive),

                InactiveStudents = await _context.Users.CountAsync(
                    u => u.UserName != "admin@campushostel.com"
                    && !u.IsActive),

                TotalHostels = await _context.Hostels.CountAsync(),

                TotalRooms = await _context.Rooms.CountAsync(),

                PendingApplications = await _context.HostelApplications.CountAsync(
                    a => a.Status == "Pending"),

                ApprovedApplications = await _context.HostelApplications.CountAsync(
                    a => a.Status == "Approved"),

                TotalAllocations = await _context.Allocations.CountAsync()
            };

            return Ok(dashboard);
        }
    }
}