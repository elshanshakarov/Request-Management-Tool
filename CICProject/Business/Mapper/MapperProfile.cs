using AutoMapper;
using Entities.Concrete;
using Entities.Dto;
using Entities.Dto.Request;
using Entities.Dto.Response;

namespace Business.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<User, RespUserDto>().ReverseMap();

            CreateMap<User, ReqUserDto>().ReverseMap();

            CreateMap<User, UserForRegisterDto>().ReverseMap();

            CreateMap<Request, ReqRequestDto>().ReverseMap();

            CreateMap<Request, RespRequestDto>()
                .ForMember(dest => dest.RequestCreatorName, opt => opt.MapFrom(src => src.Creator.Username))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.Name))
                .ForMember(dest => dest.RequestExecutorName, opt => opt.MapFrom(src => src.Executor != null ? src.Executor.Username : null))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.Name));

            CreateMap<Comment, RespCommentDto>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.User.Name + " " + src.User.Surname))
                .ForMember(dest => dest.Position, opt => opt.MapFrom(src => src.User.Position))
                .ForMember(dest => dest.Department, opt => opt.MapFrom(src => src.User.Department))
                .ForMember(dest => dest.CommentDate, opt => opt.MapFrom(src => src.Date))
                .ForMember(dest => dest.Comment, opt => opt.MapFrom(src => src.CommentText));

            CreateMap<Request, RequestDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.Name))
                .ForMember(dest => dest.CreatorFullName, opt => opt.MapFrom(src => src.Creator.Name + " " + src.Creator.Surname))
                .ForMember(dest => dest.CreatorPosition, opt => opt.MapFrom(src => src.Creator.Position))
                .ForMember(dest => dest.CreatorDepartment, opt => opt.MapFrom(src => src.Creator.Department))
                .ForMember(dest => dest.CreatorCommentDate, opt => opt.MapFrom(src => src.Date))
                .ForMember(dest => dest.CreatorComment, opt => opt.MapFrom(src => src.Text))
                .ForMember(dest => dest.RequestType, opt => opt.MapFrom(src => src.RequestType.Name))
                .ForMember(dest => dest.Priority, opt => opt.MapFrom(src => src.Priority.Name));

            CreateMap<History, RequestHistoryDto>()
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User.Name + " " + src.User.Surname))
                .ForMember(dest => dest.Position, opt => opt.MapFrom(src => src.User.Position))
                .ForMember(dest => dest.Messaga, opt => opt.MapFrom(src => src.Message))
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date));

            CreateMap<RequestInfoDto, Request>();

        }
    }
}
