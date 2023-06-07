namespace Entities.Dto
{
    public class AllRequestFilterDto
    {
        public short? CategoryId { get; set; }
        public short? StatusId { get; set; }
        public int PageSize { get; set; }
        public int Page { get; set; }
        public List<FilterDto> Filter { get; set; }

    }
}
