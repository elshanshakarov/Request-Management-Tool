using Core.DataAccess.EntityFramework.Abstract;
using Entities.Concrete;
using Entities.Dto;
using Entities.Dto.Response;

namespace DataAccess.Abstract
{
    public interface IRequestDal : IEntityRepository<Request>
    {
        IQueryable<Request> GetAllFilteredRequests(int userId, RequestFilterDto requestFilterDto);
        IQueryable<Request> GetAllMyRequests(int userId, short? statusId);
        List<CountOfRequestsDto> GetCountOfAllMyRequests(int userId, short? statusId);
        List<CountOfRequestsDto> GetCountOfAllRequests(int userId, RequestFilterDto requestFilterDto);

        IQueryable<Request> GetExecuteRequestById(int userId, int requestId);
    }
}
