using CampusHostelAllocation.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CampusHostelAllocation.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Hostel> Hostels { get; set; }

        public DbSet<Room> Rooms { get; set; }

        public DbSet<HostelApplication> HostelApplications { get; set; }

        public DbSet<Allocation> Allocations { get; set; }
    }
}