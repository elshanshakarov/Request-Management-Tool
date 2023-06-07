using Core.Utilities.Results;
using Entities.Dto;

namespace Business.Abstract
{
    public interface ICommentService
    {
        IDataResult<List<RespCommentDto>> GetAllCommentsByRequestId(int userId, int requestId);
        IResult AddComment(ReqCommentDto reqComment, int userId);
    }
}
