using CampusHostelAllocation.Data;
using CampusHostelAllocation.DTOs;
using CampusHostelAllocation.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CampusHostelAllocation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class HostelApplicationsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public HostelApplicationsController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        [HttpPost]
        public async Task<IActionResult> ApplyForHostel(HostelApplicationDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var student = await _userManager.GetUserAsync(User);

            if (student == null)
            {
                return Unauthorized("Student not found.");
            }

            var hostel = await _context.Hostels.FindAsync(model.HostelId);

            if (hostel == null)
            {
                return NotFound("Hostel not found.");
            }

            if (!string.Equals(student.Gender, hostel.Gender, StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest("You cannot apply for a hostel that does not match your gender.");
            }

            var existingApplication = await _context.HostelApplications
                .FirstOrDefaultAsync(a =>
                    a.StudentId == student.Id &&
                    a.Status == "Pending");

            if (existingApplication != null)
            {
                return BadRequest("You already have a pending hostel application.");
            }

            var application = new HostelApplication
            {
                StudentId = student.Id,
                HostelId = model.HostelId,
                ApplicationDate = DateTime.UtcNow,
                Status = "Pending"
            };

            _context.HostelApplications.Add(application);

            await _context.SaveChangesAsync();

            var response = new HostelApplicationResponseDTO
            {
                Id = application.Id,
                StudentId = student.Id,
                StudentName = student.FullName,
                HostelId = hostel.Id,
                HostelName = hostel.HostelName,
                HostelLocation = hostel.Location,
                HostelGender = hostel.Gender,
                ApplicationDate = application.ApplicationDate,
                Status = application.Status
            };

            return Ok(response);
        }
        [HttpGet("my-application")]
        public async Task<IActionResult> GetMyApplication()
        {
            var student = await _userManager.GetUserAsync(User);

            if (student == null)
            {
                return Unauthorized("Student not found.");
            }

            var application = await _context.HostelApplications
                .Include(a => a.Hostel)
                .FirstOrDefaultAsync(a => a.StudentId == student.Id);

            if (application == null)
            {
                return NotFound("You have not submitted a hostel application.");
            }

            var response = new HostelApplicationResponseDTO
            {
                Id = application.Id,
                StudentId = student.Id,
                StudentName = student.FullName,
                HostelId = application.HostelId,
                HostelName = application.Hostel.HostelName,
                HostelLocation = application.Hostel.Location,
                HostelGender = application.Hostel.Gender,
                ApplicationDate = application.ApplicationDate,
                Status = application.Status
            };

            return Ok(response);
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllApplications()
        {
            var applications = await _context.HostelApplications
                .Include(a => a.Student)
                .Include(a => a.Hostel)
                .Select(a => new HostelApplicationResponseDTO
                {
                    Id = a.Id,
                    StudentId = a.StudentId,
                    StudentName = a.Student.FullName,
                    HostelId = a.HostelId,
                    HostelName = a.Hostel.HostelName,
                    HostelLocation = a.Hostel.Location,
                    HostelGender = a.Hostel.Gender,
                    ApplicationDate = a.ApplicationDate,
                    Status = a.Status
                })
                .ToListAsync();

            return Ok(applications);
        }
        [HttpPut("{id}/status")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateApplicationStatus(
    int id,
    UpdateApplicationStatusDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (model.Status != "Approved" && model.Status != "Rejected")
            {
                return BadRequest("Status must be Approved or Rejected.");
            }

            var application = await _context.HostelApplications
                .FirstOrDefaultAsync(a => a.Id == id);

            if (application == null)
            {
                return NotFound("Application not found.");
            }

            application.Status = model.Status;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Application status updated successfully.",
                applicationId = application.Id,
                status = application.Status
            });
        }
    }

}