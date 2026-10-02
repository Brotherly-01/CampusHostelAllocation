using System.ComponentModel.DataAnnotations;

namespace CampusHostelAllocation.DTOs
{
    public class HostelApplicationDTO
    {
        [Required]
        public int HostelId { get; set; }
    }
}