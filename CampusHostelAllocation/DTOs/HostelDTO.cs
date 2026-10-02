using System.ComponentModel.DataAnnotations;

namespace CampusHostelAllocation.DTOs
{
    public class HostelDTO
    {
        [Required]
        [MaxLength(100)]
        public string HostelName { get; set; }
        
        [MaxLength(100)]
        public string Location { get; set; }

        [Required]
        [MaxLength(20)]
        public string Gender { get; set; }
    }
}