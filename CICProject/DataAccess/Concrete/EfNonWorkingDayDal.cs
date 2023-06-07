using Core.DataAccess.EntityFramework.Concrete;
using DataAccess.Abstract;
using DataAccess.Context;
using Entities.Concrete;

namespace DataAccess.Concrete
{
    public class EfNonWorkingDayDal : EfEntityRepositoryBase<NonWorkingDay, CICDbContext>, INonWorkingDayDal
    {
        public EfNonWorkingDayDal(CICDbContext context) : base(context)
        {
        }
    }
}
