using CampusHostelAllocation.Data;
using CampusHostelAllocation.DTOs;
using CampusHostelAllocation.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CampusHostelAllocation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AllocationsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AllocationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AllocateRoom(AllocationDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var student = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == model.StudentId);

            if (student == null)
            {
                return NotFound("Student not found.");
            }

            var room = await _context.Rooms
                .Include(r => r.Hostel)
                .FirstOrDefaultAsync(r => r.Id == model.RoomId);

            if (room == null)
            {
                return NotFound("Room not found.");
            }

            if (room.OccupiedSpaces >= room.Capacity)
            {
                return BadRequest("Room is already full.");
            }

            var application = await _context.HostelApplications
                .FirstOrDefaultAsync(a =>
                    a.StudentId == model.StudentId &&
                    a.Status == "Approved");

            if (application == null)
            {
                return BadRequest(
                    "Student does not have an approved hostel application.");
            }

            if (application.HostelId != room.HostelId)
            {
                return BadRequest(
                    "Room does not belong to the student's approved hostel.");
            }

            var existingAllocation = await _context.Allocations
                .FirstOrDefaultAsync(a => a.StudentId == model.StudentId);

            if (existingAllocation != null)
            {
                return BadRequest("Student has already been allocated a room.");
            }

            var allocation = new Allocation
            {
                StudentId = model.StudentId,
                RoomId = model.RoomId,
                AllocationDate = DateTime.UtcNow
            };

            _context.Allocations.Add(allocation);

            room.OccupiedSpaces++;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Room allocated successfully.",
                allocationId = allocation.Id,
                studentId = student.Id,
                studentName = student.FullName,
                roomId = room.Id,
                roomNumber = room.RoomNumber,
                hostelName = room.Hostel.HostelName,
                allocationDate = allocation.AllocationDate
            });
        }
        [HttpGet("my-allocation")]
        public async Task<IActionResult> GetMyAllocation()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Unauthorized();
            }

            var allocation = await _context.Allocations
                .Include(a => a.Room)
                .ThenInclude(r => r.Hostel)
                .FirstOrDefaultAsync(a => a.StudentId == userId);

            if (allocation == null)
            {
                return NotFound("You have not been allocated a room.");
            }

            return Ok(new
            {
                allocationId = allocation.Id,
                roomNumber = allocation.Room.RoomNumber,
                hostelName = allocation.Room.Hostel.HostelName,
                hostelLocation = allocation.Room.Hostel.Location,
                allocationDate = allocation.AllocationDate
            });
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllAllocations()
        {
            var allocations = await _context.Allocations
                .Include(a => a.Student)
                .Include(a => a.Room)
                .ThenInclude(r => r.Hostel)
                .Select(a => new
                {
                    allocationId = a.Id,
                    studentId = a.StudentId,
                    studentName = a.Student.FullName,
                    roomId = a.RoomId,
                    roomNumber = a.Room.RoomNumber,
                    hostelId = a.Room.HostelId,
                    hostelName = a.Room.Hostel.HostelName,
                    hostelLocation = a.Room.Hostel.Location,
                    allocationDate = a.AllocationDate
                })
                .ToListAsync();

            return Ok(allocations);
        }
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllocation(int id)
        {
            var allocation = await _context.Allocations
                .Include(a => a.Student)
                .Include(a => a.Room)
                .ThenInclude(r => r.Hostel)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (allocation == null)
            {
                return NotFound("Allocation not found.");
            }

            return Ok(new
            {
                allocationId = allocation.Id,
                studentId = allocation.StudentId,
                studentName = allocation.Student.FullName,
                roomId = allocation.RoomId,
                roomNumber = allocation.Room.RoomNumber,
                hostelId = allocation.Room.HostelId,
                hostelName = allocation.Room.Hostel.HostelName,
                hostelLocation = allocation.Room.Hostel.Location,
                allocationDate = allocation.AllocationDate
            });
        }
    }
}
