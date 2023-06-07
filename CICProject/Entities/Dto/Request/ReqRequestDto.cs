using Microsoft.AspNetCore.Http;

namespace Entities.Dto.Request
{
    public class ReqRequestDto
    {
        public short CategoryId { get; set; }
        public short PriorityId { get; set; }
        public short RequestTypeId { get; set; }
        public string Tittle { get; set; }
        public string Text { get; set; }
        public IFormFile? File { get; set; }

    }
}
