using System.ComponentModel.DataAnnotations;

namespace CampusHostelAllocation.DTOs
{
    public class RoomDTO
    {
        [Required]
        [MaxLength(20)]
        public string RoomNumber { get; set; }

        [Required]
        public int Capacity { get; set; }

        [Required]
        public int HostelId { get; set; }
    }
}