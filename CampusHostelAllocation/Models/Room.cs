using System.ComponentModel.DataAnnotations;

namespace CampusHostelAllocation.Models
{
    public class Room
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string RoomNumber { get; set; }

        [Required]
        public int Capacity { get; set; }

        public int HostelId { get; set; }

        public Hostel Hostel { get; set; }

        public int OccupiedSpaces { get; set; }
    }
}