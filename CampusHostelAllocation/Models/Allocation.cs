using System.ComponentModel.DataAnnotations;

namespace CampusHostelAllocation.Models
{
    public class Allocation
    {
        public int Id { get; set; }

        [Required]
        public string StudentId { get; set; }

        public ApplicationUser Student { get; set; }

        [Required]
        public int RoomId { get; set; }

        public Room Room { get; set; }

        public DateTime AllocationDate { get; set; } = DateTime.UtcNow;
    }
}
