using Autofac;
using Business.Abstract;
using Business.Concrete;
using Core.Utilities.Security.JWT;
using DataAccess.Abstract;
using DataAccess.Concrete;

namespace Business.DependencyResolvers.Autofac
{
    public class AutofacBusinessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<EfUserDal>().As<IUserDal>().SingleInstance();
            builder.RegisterType<UserManager>().As<IUserService>().SingleInstance();

            builder.RegisterType<EfRequestDal>().As<IRequestDal>().SingleInstance();
            builder.RegisterType<RequestManager>().As<IRequestService>().SingleInstance();

            builder.RegisterType<UserManager>().As<IUserService>();
            builder.RegisterType<EfUserDal>().As<IUserDal>();

            builder.RegisterType<AuthManager>().As<IAuthService>();
            builder.RegisterType<JwtHelper>().As<ITokenHelper>();

            builder.RegisterType<EfCategoryDal>().As<ICategoryDal>();

            builder.RegisterType<EfCategoryUserDal>().As<ICategoryUserDal>();

            builder.RegisterType<EfCommentDal>().As<ICommentDal>();
            builder.RegisterType<EfHistoryDal>().As<IHistoryDal>();


        }
    }
}
