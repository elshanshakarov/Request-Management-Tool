using Core.DataAccess.EntityFramework.Concrete;
using DataAccess.Abstract;
using DataAccess.Context;
using Entities.Concrete;

namespace DataAccess.Concrete
{
    public class EfHistoryDal : EfEntityRepositoryBase<History, CICDbContext>, IHistoryDal
    {
        public EfHistoryDal(CICDbContext context) : base(context)
        {
        }
    }
}
