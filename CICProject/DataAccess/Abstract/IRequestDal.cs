using Core.DataAccess.EntityFramework.Abstract;
using Entities.Concrete;
using Entities.Dto;

namespace DataAccess.Abstract
{
    public interface IRequestDal : IEntityRepository<Request>
    {
        IQueryable<Request> GetRequestById(int userId, int requestId);
        IQueryable<Request> GetAllFilteredRequests(int userId, AllRequestFilterDto requestFilterDto);
        IQueryable<Request> GetAllMyRequests(int userId, short? statusId);
        List<CountOfRequestsDto> GetCountOfAllMyRequests(int userId, short? statusId);
        List<CountOfRequestsDto> GetCountOfAllRequests(int userId, AllRequestFilterDto requestFilterDto);
    }
}
