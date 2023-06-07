using Core.DataAccess.EntityFramework.Concrete;
using DataAccess.Abstract;
using DataAccess.Context;
using Entities.Concrete;
using Entities.Dto;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Concrete
{
    public class EfRequestDal : EfEntityRepositoryBase<Request, CICDbContext>, IRequestDal
    {
        private readonly CICDbContext _context;

        public EfRequestDal(CICDbContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable<Request> GetRequestById(int userId, int requestId)
        {
            var result = _context.Requests
                    .Include(p => p.Category)
                    .ThenInclude(p => p.CategoryUsers)
                    .Where(p => (p.Category.CategoryUsers.Count(c => c.UserId == userId && c.ExecutePermission) > 0 || p.CreatorId == userId) && p.Id == requestId);

            return result;
        }

        public IQueryable<Request> GetAllFilteredRequests(int userId, AllRequestFilterDto requestFilterDto)
        {
            //select distinct r.id, r.CreatorId,r.ExecutorId, r.CategoryId, r.StatusId from Requests r
            //join CategoryUsers cu on r.CategoryId = cu.CategoryId
            //where cu.UserId = 2 and cu.ExecutePermission = 'true' or r.CreatorId = 2
            //order by r.Id desc
            IQueryable<Request> result = null;

            if (requestFilterDto.StatusId == null && requestFilterDto.CategoryId == null)
            {
                result = _context.Requests
                    .Include(p => p.Category)
                    .ThenInclude(p => p.CategoryUsers)
                    .Where(p => p.Category.CategoryUsers.Count(c => c.UserId == userId && c.ExecutePermission) > 0 || p.CreatorId == userId)
                    .OrderByDescending(p => p.Id);
            }
            else if (requestFilterDto.StatusId != null && requestFilterDto.CategoryId != null)
            {
                result = _context.Requests
                    .Include(p => p.Category)
                    .ThenInclude(p => p.CategoryUsers)
                    .Where(p => (p.Category.CategoryUsers.Count(c => c.UserId == userId && c.ExecutePermission) > 0 || p.CreatorId == userId) && p.CategoryId == requestFilterDto.CategoryId && p.StatusId == requestFilterDto.StatusId)
                    .OrderByDescending(p => p.Id);
            }
            else if (requestFilterDto.StatusId == null && requestFilterDto.CategoryId != null)
            {
                result = _context.Requests
                    .Include(p => p.Category)
                    .ThenInclude(p => p.CategoryUsers)
                    .Where(p => (p.Category.CategoryUsers.Count(c => c.UserId == userId && c.ExecutePermission) > 0 || p.CreatorId == userId) && p.CategoryId == requestFilterDto.CategoryId)
                    .OrderByDescending(p => p.Id);
            }
            else if (requestFilterDto.StatusId != null && requestFilterDto.CategoryId == null)
            {
                result = _context.Requests
                    .Include(p => p.Category)
                    .ThenInclude(p => p.CategoryUsers)
                    .Where(p => (p.Category.CategoryUsers.Count(c => c.UserId == userId && c.ExecutePermission) > 0 || p.CreatorId == userId) && p.StatusId == requestFilterDto.StatusId)
                    .OrderByDescending(p => p.Id);
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
                result = _context.Requests.Include(e => e.Status).Include(e => e.Category).ThenInclude(e => e.CategoryUsers.Where(cu => cu.UserId == userId && (cu.ExecutePermission || cu.CreatePermission)))
                    .Where(r => r.Category.CategoryUsers.Count > 0 && (r.CreatorId == userId || r.ExecutorId == userId)).OrderByDescending(r => r.Id);
            }
            else if (statusId != null)
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

        public List<CountOfRequestsDto> GetCountOfAllRequests(int userId, AllRequestFilterDto requestFilterDto)
        {
            IOrderedQueryable<Request> result = null;
            List<CountOfRequestsDto> countDto = null;
            if (requestFilterDto.StatusId == null && requestFilterDto.CategoryId == null)
            {
                result = _context.Requests
                    .Include(p => p.Category)
                    .ThenInclude(p => p.CategoryUsers)
                    .Where(p => p.Category.CategoryUsers.Count(c => c.UserId == userId && c.ExecutePermission) > 0 || p.CreatorId == userId)
                    .OrderByDescending(p => p.Id);

                countDto = result.GroupBy(e => e.Status.Name).Select(grp => new CountOfRequestsDto() { Status = grp.Key, Count = grp.Count() }).ToList();
                countDto.Add(new CountOfRequestsDto() { Status = "Hamisi", Count = countDto.Sum(x => x.Count) });
            }
            else if (requestFilterDto.StatusId != null && requestFilterDto.CategoryId != null)
            {
                result = _context.Requests
                    .Include(p => p.Category)
                    .ThenInclude(p => p.CategoryUsers)
                    .Where(p => (p.Category.CategoryUsers.Count(c => c.UserId == userId && c.ExecutePermission) > 0 || p.CreatorId == userId) && p.CategoryId == requestFilterDto.CategoryId && p.StatusId == requestFilterDto.StatusId)
                    .OrderByDescending(p => p.Id);

                countDto = result.GroupBy(e => e.Status.Name).Select(grp => new CountOfRequestsDto() { Status = grp.Key, Count = grp.Count() }).ToList();
            }
            else if (requestFilterDto.StatusId == null && requestFilterDto.CategoryId != null)
            {
                result = _context.Requests
                    .Include(p => p.Category)
                    .ThenInclude(p => p.CategoryUsers)
                    .Where(p => (p.Category.CategoryUsers.Count(c => c.UserId == userId && c.ExecutePermission) > 0 || p.CreatorId == userId) && p.CategoryId == requestFilterDto.CategoryId)
                    .OrderByDescending(p => p.Id);

                countDto = result.GroupBy(e => e.Status.Name).Select(grp => new CountOfRequestsDto() { Status = grp.Key, Count = grp.Count() }).ToList();
            }
            else if (requestFilterDto.StatusId != null && requestFilterDto.CategoryId == null)
            {
                result = _context.Requests
                    .Include(p => p.Category)
                    .ThenInclude(p => p.CategoryUsers)
                    .Where(p => (p.Category.CategoryUsers.Count(c => c.UserId == userId && c.ExecutePermission) > 0 || p.CreatorId == userId) && p.StatusId == requestFilterDto.StatusId)
                    .OrderByDescending(p => p.Id);

                countDto = result.GroupBy(e => e.Status.Name).Select(grp => new CountOfRequestsDto() { Status = grp.Key, Count = grp.Count() }).ToList();
            }



            return countDto;
        }
    }
}
