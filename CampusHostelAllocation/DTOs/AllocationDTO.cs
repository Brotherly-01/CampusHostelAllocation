using System.ComponentModel.DataAnnotations;

namespace CampusHostelAllocation.DTOs
{
    public class AllocationDTO
    {
        [Required]
        public string StudentId { get; set; }

        [Required]
        public int RoomId { get; set; }
    }
}