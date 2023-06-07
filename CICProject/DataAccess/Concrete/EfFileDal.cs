using Core.DataAccess.EntityFramework.Concrete;
using DataAccess.Abstract;
using DataAccess.Context;
using File = Entities.Concrete.File;

namespace DataAccess.Concrete
{
    public class EfFileDal : EfEntityRepositoryBase<File, CICDbContext>, IFileDal
    {
        public EfFileDal(CICDbContext context) : base(context)
        {
        }
    }
}
