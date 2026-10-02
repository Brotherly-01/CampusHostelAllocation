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
    public class HostelsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public HostelsController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateHostel(HostelDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var hostel = new Hostel
            {
                HostelName = model.HostelName,
                Location = model.Location,
                Gender = model.Gender
            };

            _context.Hostels.Add(hostel);

            await _context.SaveChangesAsync();

            return Ok(hostel);
        }

        [HttpGet]
        public async Task<IActionResult> GetHostels()
        {
            var hostels = await _context.Hostels.ToListAsync();

            return Ok(hostels);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetHostel(int id)
        {
            var hostel = await _context.Hostels.FindAsync(id);

            if (hostel == null)
            {
                return NotFound("Hostel not found.");
            }

            return Ok(hostel);
        }
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateHostel(int id, HostelDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var hostel = await _context.Hostels.FindAsync(id);

            if (hostel == null)
            {
                return NotFound("Hostel not found.");
            }

            hostel.HostelName = model.HostelName;
            hostel.Location = model.Location;
            hostel.Gender = model.Gender;

            await _context.SaveChangesAsync();

            return Ok(hostel);
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteHostel(int id)
        {
            var hostel = await _context.Hostels.FindAsync(id);

            if (hostel == null)
            {
                return NotFound("Hostel not found.");
            }

            _context.Hostels.Remove(hostel);

            await _context.SaveChangesAsync();

            return Ok("Hostel deleted successfully.");
        }
    }
}