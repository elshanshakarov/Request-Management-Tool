using Entities.Abstract;
using System.Linq.Expressions;

namespace Core.DataAccess.EntityFramework.Abstract
{
    //"Generic Repository Design Pattern"
    //Generic Constraint vasitesile generic-i serhedleyirik.
    //T yalniz referance type-da olmaidir ve IEntity-i implement eden class olmalidir
    //class --> referance type ola biler
    //IEntity --> IEntity yaxud IEntity-i implement eden class ola biler
    //new() --> new ile yaradila bilen olmalidir.IEntity interface-nin icerisi bos oldugundan bizim hec bir isimize yaramir.
    //IEntity-de interface oldugundan new ile yaradila bilmediyi ucun IEntity yaza bilmesinin qarsisin aldiq. 
    public interface IEntityRepository<T> where T : class, IEntity, new()
    {
        //filter=null o demekdir ki, filter vermeyede bilerik.
        //Filter vermesek butun datani getirecek, lakin filter versek bize filter-lenmis datani getirecek
        IQueryable<T> GetAll(Expression<Func<T, bool>> filter = null);
        IQueryable<T> Get(Expression<Func<T, bool>> filter);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
