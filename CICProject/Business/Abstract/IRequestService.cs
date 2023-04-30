using Core.Utilities.Results;
using Entities.Concrete;
using Entities.Dto;
using Entities.Dto.Request;
using Entities.Dto.Response;

namespace Business.Abstract
{
    public interface IRequestService
    {

        IDataResult<List<RespRequestDto>> GetAllFilteredRequests(int userId, RequestFilterDto requestFilterDto);
        IDataResult<List<RespRequestDto>> GetAllMyRequests(int userId, short? statusId);
        IDataResult<List<CountOfRequestsDto>> GetCountOfAllMyRequests(int userId, short? statusId);
        IDataResult<List<CountOfRequestsDto>> GetCountOfAllRequests(int userId, RequestFilterDto requestFilterDto);
        IDataResult<RequestByIdRequestDto> GetRequestByIdRequestDto(int userId, int requestId);
        IDataResult<List<CommentDto>> GetRequestByIdCommentDto(int userId, int requestId);
        IResult AddComment(int userId, int requestId, string commentText);
        IResult ChangeStatus(int requestId, int userId, short statusId);




        IResult Add(Request request);
        IResult Update(ReqRequestDto request);
        IResult Delete(int id);




    }
}
