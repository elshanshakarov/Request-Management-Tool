using Microsoft.AspNetCore.Http;

namespace Entities.Dto
{
    public class ReqCommentDto
    {
        public int RequestId { get; set; }
        public string CommentText { get; set; }
        public IFormFile? File { get; set; }
    }
}
