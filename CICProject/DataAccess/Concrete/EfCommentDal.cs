using Core.DataAccess.EntityFramework.Concrete;
using DataAccess.Abstract;
using DataAccess.Context;
using Entities.Concrete;

namespace DataAccess.Concrete
{
    public class EfCommentDal : EfEntityRepositoryBase<Comment, CICDbContext>, ICommentDal
    {
        public EfCommentDal(CICDbContext context) : base(context)
        {
        }
    }
}
