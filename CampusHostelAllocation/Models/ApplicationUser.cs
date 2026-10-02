using Microsoft.AspNetCore.Identity;

namespace CampusHostelAllocation.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }

        public string Gender { get; set; }

        public bool IsActive { get; set; } = true;
    }
}