namespace Entities.Dto
{
    public class RequestDto
    {
        public string Status { get; set; }
        public string CreatorFullName { get; set; }
        public string CreatorPosition { get; set; }
        public string CreatorDepartment { get; set; }
        public DateTime CreatorCommentDate { get; set; }
        public string CreatorComment { get; set; }
        public string RequestType { get; set; }
        public string Priority { get; set; }
        public RespCommentDto? LastComment { get; set; }
    }
}
