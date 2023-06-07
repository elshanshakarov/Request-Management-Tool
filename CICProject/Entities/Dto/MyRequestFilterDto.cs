namespace Entities.Dto
{
    public class MyRequestFilterDto
    {
        public short? StatusId { get; set; }
        public int PageSize { get; set; }
        public int Page { get; set; }
        public List<FilterDto> Filter { get; set; }
    }
}
