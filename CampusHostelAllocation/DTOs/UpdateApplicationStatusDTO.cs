using System.ComponentModel.DataAnnotations;

namespace CampusHostelAllocation.DTOs
{
    public class UpdateApplicationStatusDTO
    {
        [Required]
        public string Status { get; set; }
    }
}