namespace CampusHostelAllocation.DTOs
{
    public class HostelApplicationResponseDTO
    {
        public int Id { get; set; }

        public string StudentId { get; set; }

        public string StudentName { get; set; }

        public int HostelId { get; set; }

        public string HostelName { get; set; }

        public string HostelLocation { get; set; }

        public string HostelGender { get; set; }

        public DateTime ApplicationDate { get; set; }

        public string Status { get; set; }
    }
}