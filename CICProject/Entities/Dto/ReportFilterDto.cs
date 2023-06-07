namespace Entities.Dto
{
    public class ReportFilterDto
    {
        public string StartDateRange { get; set; }
        public string EndDateRange { get; set; }
        public int PageSize { get; set; }
        public int Page { get; set; }
        public List<FilterDto> Filter { get; set; }
    }
}
