using Autofac;
using Business.Abstract;
using Business.Concrete;
using Core.Utilities.Helpers;
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

            builder.RegisterType<EfCommentDal>().As<ICommentDal>();
            builder.RegisterType<CommentManager>().As<ICommentService>();

            builder.RegisterType<EfHistoryDal>().As<IHistoryDal>();
            builder.RegisterType<HistoryManager>().As<IHistoryService>();

            builder.RegisterType<AuthManager>().As<IAuthService>();
            builder.RegisterType<JwtHelper>().As<ITokenHelper>();

            builder.RegisterType<EfCategoryDal>().As<ICategoryDal>();

            builder.RegisterType<EfCategoryUserDal>().As<ICategoryUserDal>();

            builder.RegisterType<StatusManager>().As<IStatusService>();

            builder.RegisterType<EfNonWorkingDayDal>().As<INonWorkingDayDal>();

            builder.RegisterType<EfFileDal>().As<IFileDal>();
            builder.RegisterType<FileHelperManager>().As<IFileHelper>();

        }
    }
}
