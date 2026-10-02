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
    [Authorize(Roles = "Admin")]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        private readonly UserManager<ApplicationUser> _userManager;

        public UsersController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;

            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _context.Users
                .Where(u => u.UserName != "admin@campushostel.com")
                .Select(u => new
                {
                    id = u.Id,
                    fullName = u.FullName,
                    email = u.Email,
                    phoneNumber = u.PhoneNumber,
                    gender = u.Gender,
                    isActive = u.IsActive
                })
                .ToListAsync();

            return Ok(users);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(string id)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            return Ok(new
            {
                id = user.Id,
                fullName = user.FullName,
                email = user.Email,
                phoneNumber = user.PhoneNumber,
                gender = user.Gender
            });
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(
        string id,
        UpdateUserDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            if (user.UserName == "admin@campushostel.com")
            {
                return BadRequest(
                    "The main administrator account cannot be modified.");
            }

            var existingEmailUser = await _userManager.FindByEmailAsync(
                model.Email);

            if (existingEmailUser != null &&
                existingEmailUser.Id != user.Id)
            {
                return BadRequest("Another user already has this email address.");
            }

            user.FullName = model.FullName;
            user.PhoneNumber = model.PhoneNumber;
            user.Gender = model.Gender;

            if (!string.Equals(
                user.Email,
                model.Email,
                StringComparison.OrdinalIgnoreCase))
            {
                var emailResult = await _userManager.SetEmailAsync(
                    user,
                    model.Email);

                if (!emailResult.Succeeded)
                {
                    return BadRequest(emailResult.Errors);
                }

                var usernameResult = await _userManager.SetUserNameAsync(
                    user,
                    model.Email);

                if (!usernameResult.Succeeded)
                {
                    return BadRequest(usernameResult.Errors);
                }
            }

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok(new
            {
                message = "User updated successfully.",
                id = user.Id,
                fullName = user.FullName,
                email = user.Email,
                phoneNumber = user.PhoneNumber,
                gender = user.Gender
            });
        }
        
        [HttpPut("{id}/deactivate")]
        public async Task<IActionResult> DeactivateUser(string id)
                {
                    var user = await _userManager.FindByIdAsync(id);

                    if (user == null)
                    {
                        return NotFound("User not found.");
                    }

                    if (user.UserName == "admin@campushostel.com")
                    {
                        return BadRequest(
                            "The main administrator account cannot be deactivated.");
                    }

                    user.IsActive = false;

                    var result = await _userManager.UpdateAsync(user);

                    if (!result.Succeeded)
                    {
                        return BadRequest(result.Errors);
                    }

                    return Ok(new
                    {
                        message = "User deactivated successfully.",
                        id = user.Id,
                        fullName = user.FullName,
                        isActive = user.IsActive
                    });
                }

        [HttpPut("{id}/activate")]
        public async Task<IActionResult> ActivateUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);


            if (user == null)
            {
                return NotFound("User not found.");
            }

            if (user.UserName == "admin@campushostel.com")
            {
                return BadRequest(
                    "The main administrator account cannot be modified.");
            }

            user.IsActive = true;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok(new
            {
                message = "User activated successfully.",
                id = user.Id,
                fullName = user.FullName,
                isActive = user.IsActive
            });


        }

    }
}