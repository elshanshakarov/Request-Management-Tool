using Core.DataAccess.EntityFramework.Concrete;
using DataAccess.Abstract;
using DataAccess.Context;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete
{
    public class EfHistoryDal : EfEntityRepositoryBase<History, CICDbContext>, IHistoryDal
    {
        public EfHistoryDal(CICDbContext context) : base(context)
        {
        }
    }
}
