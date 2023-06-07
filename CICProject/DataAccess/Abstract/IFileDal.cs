using Core.DataAccess.EntityFramework.Abstract;
using File = Entities.Concrete.File;

namespace DataAccess.Abstract
{
    public interface IFileDal : IEntityRepository<File>
    {
    }
}
