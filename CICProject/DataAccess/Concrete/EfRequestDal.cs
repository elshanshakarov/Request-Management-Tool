using Core.DataAccess.EntityFramework.Concrete;
using Core.Utilities.Results;
using DataAccess.Abstract;
using DataAccess.Context;
using Entities.Concrete;
using Entities.Dto;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace DataAccess.Concrete
{
    public class EfRequestDal : EfEntityRepositoryBase<Request, CICDbContext>, IRequestDal
    {
        private readonly CICDbContext _context;


        public EfRequestDal(CICDbContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable<Request> GetAllFilteredRequests(int userId, RequestFilterDto requestFilterDto)
        {
            //select distinct r.id, r.CreatorId,r.ExecutorId, r.CategoryId, r.StatusId from Requests r
            //join CategoryUsers cu on r.CategoryId = cu.CategoryId
            //where cu.UserId = 2 and cu.ExecutePermission = 'true' or r.CreatorId = 2
            //order by r.Id desc
            IQueryable<Request> result = null;

            if (requestFilterDto.StatusId == null && requestFilterDto.CategoryId == null)
            {
                result = (from r in _context.Requests
                          join cu in _context.CategoryUsers on r.CategoryId equals cu.CategoryId
                          where cu.UserId == userId && cu.ExecutePermission || r.CreatorId == userId
                          select r).Distinct().OrderByDescending(r => r.Id);
                //result = _context.Requests.Include(e => e.Status).Include(e => e.Category).ThenInclude(e => e.CategoryUsers.Where(cu => cu.UserId == userId && cu.ExecutePermission))
                //        .Where(r => r.Category.CategoryUsers.Count > 0 && r.CreatorId == userId).OrderByDescending(r => r.Id);
            }
            else if (requestFilterDto.StatusId != null && requestFilterDto.CategoryId != null)
            {
                result = (from r in _context.Requests
                          join cu in _context.CategoryUsers on r.CategoryId equals cu.CategoryId
                          where cu.UserId == userId && (cu.ExecutePermission || r.CreatorId == userId) && r.CategoryId == requestFilterDto.CategoryId && r.StatusId == requestFilterDto.StatusId
                          select r).Distinct().OrderByDescending(r => r.Id);
            }
            else if (requestFilterDto.StatusId == null && requestFilterDto.CategoryId != null)
            {
                result = (from r in _context.Requests
                          join cu in _context.CategoryUsers on r.CategoryId equals cu.CategoryId
                          where cu.UserId == userId && (cu.ExecutePermission || r.CreatorId == userId) && r.CategoryId == requestFilterDto.CategoryId
                          select r).Distinct().OrderByDescending(r => r.Id);
            }
            else if (requestFilterDto.StatusId != null && requestFilterDto.CategoryId == null)
            {
                result = (from r in _context.Requests
                          join cu in _context.CategoryUsers on r.CategoryId equals cu.CategoryId
                          where cu.UserId == userId && (cu.ExecutePermission || r.CreatorId == userId) && r.StatusId == requestFilterDto.StatusId
                          select r).Distinct().OrderByDescending(r => r.Id);
            }

            return result;
        }

        public IQueryable<Request> GetAllMyRequests(int userId, short? statusId)
        {
            //select distinct r.id, r.CreatorId,r.ExecutorId, r.CategoryId, r.StatusId from Requests r
            //join CategoryUsers cu on r.CategoryId = cu.CategoryId
            //where(cu.UserId = 2 and(cu.ExecutePermission = 'true' or cu.CreatePermission = 'true')) and(r.CreatorId = 2 or r.ExecutorId = 2)
            //order by r.Id desc
            IQueryable<Request> result = null;

            if (statusId == null)
            {
                result = _context.Requests.Include(e => e.Status).Include(e => e.Category).ThenInclude(e => e.CategoryUsers.Where(cu => cu.UserId == userId && cu.ExecutePermission || cu.CreatePermission))
                    .Where(r => r.Category.CategoryUsers.Count > 0 && (r.CreatorId == userId || r.ExecutorId == userId)).OrderByDescending(r => r.Id);
            }
            else if (statusId != null) //aciq
            {
                result = _context.Requests.Include(e => e.Status).Include(e => e.Category).ThenInclude(e => e.CategoryUsers.Where(cu => cu.UserId == userId && cu.ExecutePermission || cu.CreatePermission))
                    .Where(r => r.Category.CategoryUsers.Count > 0 && (r.CreatorId == userId || r.ExecutorId == userId) && r.StatusId == statusId).OrderByDescending(r => r.Id);
            }

            return result;
        }

        public List<CountOfRequestsDto> GetCountOfAllMyRequests(int userId, short? statusId)
        {
            List<CountOfRequestsDto> countDto = new List<CountOfRequestsDto>();

            if (statusId == null)
            {
                var result = _context.Requests.Include(e => e.Status).Include(e => e.Category).ThenInclude(e => e.CategoryUsers.Where(cu => cu.UserId == userId && cu.ExecutePermission))
                      .Where(r => r.Category.CategoryUsers.Count > 0 && (r.CreatorId == userId || r.ExecutorId == userId)).OrderByDescending(r => r.Id);

                countDto = result.GroupBy(e => e.Status.Name).Select(grp => new CountOfRequestsDto() { Status = grp.Key, Count = grp.Count() }).ToList();
                countDto.Add(new CountOfRequestsDto() { Status = "Hamisi", Count = countDto.Sum(x => x.Count) });
            }
            else
            {
                var result = _context.Requests.Include(e => e.Status).Include(e => e.Category).ThenInclude(e => e.CategoryUsers.Where(cu => cu.UserId == userId && cu.ExecutePermission))
                     .Where(r => r.Category.CategoryUsers.Count > 0 && (r.CreatorId == userId || r.ExecutorId == userId) && r.StatusId == statusId).OrderByDescending(r => r.Id);

                countDto = result.GroupBy(e => e.Status.Name).Select(grp => new CountOfRequestsDto() { Status = grp.Key, Count = grp.Count() }).ToList();
            }

            return countDto;
        }

        public List<CountOfRequestsDto> GetCountOfAllRequests(int userId, RequestFilterDto requestFilterDto)
        {
            IOrderedQueryable<Request> result = null;
            List<CountOfRequestsDto> countDto=null;
            if (requestFilterDto.StatusId == null && requestFilterDto.CategoryId == null)
            {
                result = (from r in _context.Requests
                          join cu in _context.CategoryUsers on r.CategoryId equals cu.CategoryId
                          where cu.UserId == userId && cu.ExecutePermission || r.CreatorId == userId
                          select r).Distinct().OrderByDescending(r => r.Id);
                countDto = result.GroupBy(e => e.Status.Name).Select(grp => new CountOfRequestsDto() { Status = grp.Key, Count = grp.Count() }).ToList();
                countDto.Add(new CountOfRequestsDto() { Status = "Hamisi", Count = countDto.Sum(x => x.Count) });
            }
            else if (requestFilterDto.StatusId != null && requestFilterDto.CategoryId != null)
            {
                result = (from r in _context.Requests
                          join cu in _context.CategoryUsers on r.CategoryId equals cu.CategoryId
                          where cu.UserId == userId && (cu.ExecutePermission || r.CreatorId == userId) && r.CategoryId == requestFilterDto.CategoryId && r.StatusId == requestFilterDto.StatusId
                          select r).Distinct().OrderByDescending(r => r.Id);
                countDto = result.GroupBy(e => e.Status.Name).Select(grp => new CountOfRequestsDto() { Status = grp.Key, Count = grp.Count() }).ToList();
            }
            else if (requestFilterDto.StatusId == null && requestFilterDto.CategoryId != null)
            {
                result = (from r in _context.Requests
                          join cu in _context.CategoryUsers on r.CategoryId equals cu.CategoryId
                          where cu.UserId == userId && (cu.ExecutePermission || r.CreatorId == userId) && r.CategoryId == requestFilterDto.CategoryId
                          select r).Distinct().OrderByDescending(r => r.Id);
                countDto = result.GroupBy(e => e.Status.Name).Select(grp => new CountOfRequestsDto() { Status = grp.Key, Count = grp.Count() }).ToList();
            }
            else if (requestFilterDto.StatusId != null && requestFilterDto.CategoryId == null)
            {
                result = (from r in _context.Requests
                          join cu in _context.CategoryUsers on r.CategoryId equals cu.CategoryId
                          where cu.UserId == userId && (cu.ExecutePermission || r.CreatorId == userId) && r.StatusId == requestFilterDto.StatusId
                          select r).Distinct().OrderByDescending(r => r.Id);
                countDto = result.GroupBy(e => e.Status.Name).Select(grp => new CountOfRequestsDto() { Status = grp.Key, Count = grp.Count() }).ToList();
            }



            return countDto;
        }















        public IQueryable<Request> GetExecuteRequestById(int userId, int requestId)
        {
            //var result = from r in _context.Requests
            //             join cu in _context.CategoryUsers on r.CategoryId equals cu.CategoryId
            //             where cu.ExecutePermission == true && cu.UserId == userId && r.Id == requestId
            //             select r;
            var result = (from r in _context.Requests
                          join cu in _context.CategoryUsers on r.CategoryId equals cu.CategoryId
                          where cu.UserId == userId && cu.ExecutePermission == true && r.CreatorId != userId
                          select r)
                       .Concat(from r in _context.Requests where r.CreatorId == userId select r)
                       .Where(p => p.Id == requestId);

            return result;
        }

















        //---------------------------------------------------------------------------------------------------------------






    }
}
