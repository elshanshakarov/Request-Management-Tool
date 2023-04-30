namespace Entities.Dto.Request
{
    public class ReqRequestDto
    {
        public int Id { get; set; }
        public short CategoryId { get; set; }
        public short PriorityId { get; set; }
        public short RequestTypeId { get; set; }
        public string Tittle { get; set; }
        public string Text { get; set; }
        public short StatusId { get; set; }

    }
}
