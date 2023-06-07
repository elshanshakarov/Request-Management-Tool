namespace Entities.Dto
{
    public class ExcelFilterDto
    {
        public string StartDateRange { get; set; }
        public string EndDateRange { get; set; }
        public List<FilterDto> Filter { get; set; }
    }
}
