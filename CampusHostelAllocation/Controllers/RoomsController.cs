using CampusHostelAllocation.Data;
using CampusHostelAllocation.DTOs;
using CampusHostelAllocation.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CampusHostelAllocation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class RoomsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public RoomsController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpPost]
        public async Task<IActionResult> CreateRoom(RoomDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var hostel = await _context.Hostels.FindAsync(model.HostelId);

            if (hostel == null)
            {
                return NotFound("Hostel not found.");
            }

            var room = new Room
            {
                RoomNumber = model.RoomNumber,
                Capacity = model.Capacity,
                HostelId = model.HostelId,
                OccupiedSpaces = 0
            };

            _context.Rooms.Add(room);

            await _context.SaveChangesAsync();

            return Ok(room);
        }

        [HttpGet]
        public async Task<IActionResult> GetRooms()
        {
            var rooms = await _context.Rooms
                .Include(r => r.Hostel)
                .ToListAsync();

            return Ok(rooms);
        }
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetRoom(int id)
        {
            var room = await _context.Rooms
                .Include(r => r.Hostel)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (room == null)
            {
                return NotFound("Room not found.");
            }

            return Ok(room);
        }
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateRoom(int id, RoomDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var room = await _context.Rooms.FindAsync(id);

            if (room == null)
            {
                return NotFound("Room not found.");
            }

            var hostel = await _context.Hostels.FindAsync(model.HostelId);

            if (hostel == null)
            {
                return NotFound("Hostel not found.");
            }

            room.RoomNumber = model.RoomNumber;
            room.Capacity = model.Capacity;
            room.HostelId = model.HostelId;

            await _context.SaveChangesAsync();

            return Ok(room);
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteRoom(int id)
        {
            var room = await _context.Rooms.FindAsync(id);

            if (room == null)
            {
                return NotFound("Room not found.");
            }

            _context.Rooms.Remove(room);

            await _context.SaveChangesAsync();

            return Ok("Room deleted successfully.");
        }
    }
}