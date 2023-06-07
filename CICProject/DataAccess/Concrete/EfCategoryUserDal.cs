using Core.DataAccess.EntityFramework.Concrete;
using DataAccess.Abstract;
using DataAccess.Context;
using Entities.Concrete;

namespace DataAccess.Concrete
{
    public class EfCategoryUserDal : EfEntityRepositoryBase<CategoryUser, CICDbContext>, ICategoryUserDal
    {
        public EfCategoryUserDal(CICDbContext context) : base(context)
        {
        }
    }
}
