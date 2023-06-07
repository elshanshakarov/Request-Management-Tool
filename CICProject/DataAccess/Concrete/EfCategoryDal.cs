using Core.DataAccess.EntityFramework.Concrete;
using DataAccess.Abstract;
using DataAccess.Context;
using Entities.Concrete;

namespace DataAccess.Concrete
{
    public class EfCategoryDal : EfEntityRepositoryBase<Category, CICDbContext>, ICategoryDal
    {
        public EfCategoryDal(CICDbContext context) : base(context)
        {
        }
    }
}
