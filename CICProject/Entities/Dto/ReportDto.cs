namespace Entities.Dto
{
    public class ReportDto
    {
        public int RequestId { get; set; }
        public string Creator { get; set; }
        public string Category { get; set; }
        public String CreationDate { get; set; }
        public String? ExecutionDate { get; set; }
        public double? ExecutionPeriod { get; set; }
        public string Executor { get; set; }
        public String? ClosingDate { get; set; }
        public string Status { get; set; }
    }
}
