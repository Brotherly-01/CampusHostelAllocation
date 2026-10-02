namespace CampusHostelAllocation.DTOs
{
    public class AdminDashboardDTO
    {
        public int TotalStudents { get; set; }

        public int ActiveStudents { get; set; }

        public int InactiveStudents { get; set; }

        public int TotalHostels { get; set; }

        public int TotalRooms { get; set; }

        public int PendingApplications { get; set; }

        public int ApprovedApplications { get; set; }

        public int TotalAllocations { get; set; }
    }
}