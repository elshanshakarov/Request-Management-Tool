using Core.DataAccess.EntityFramework.Concrete;
using DataAccess.Abstract;
using DataAccess.Context;
using Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DataAccess.Concrete
{
    public class EfUserDal : EfEntityRepositoryBase<User, CICDbContext>, IUserDal
    {
        private readonly CICDbContext _context;

        public EfUserDal(CICDbContext context) : base(context)
        {
            _context = context;
        }


        public List<OperationClaim> GetClaims(User user)
        {
            var result = from operationClaim in _context.OperationClaims
                         join userOperationClaim in _context.UserOperationClaims
                         on operationClaim.Id equals userOperationClaim.OperationClaim.Id
                         where userOperationClaim.User.Id == user.Id
                         select new OperationClaim { Id = operationClaim.Id, Name = operationClaim.Name };

           

            return result.ToList();
        }

    }
}
