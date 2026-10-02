using System.ComponentModel.DataAnnotations;

namespace CampusHostelAllocation.DTOs
{
    public class ForgotPasswordDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
