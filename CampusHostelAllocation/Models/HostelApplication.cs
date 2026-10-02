using System.ComponentModel.DataAnnotations;

namespace CampusHostelAllocation.Models
{
    public class HostelApplication
    {
        public int Id { get; set; }

        [Required]
        public string StudentId { get; set; }

        public ApplicationUser Student { get; set; }

        [Required]
        public int HostelId { get; set; }

        public Hostel Hostel { get; set; }

        public DateTime ApplicationDate { get; set; } = DateTime.UtcNow;

        [Required]
        [MaxLength(20)]
        public string Status { get; set; } = "Pending";
    }
}