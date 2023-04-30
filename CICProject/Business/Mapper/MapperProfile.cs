using AutoMapper;
using Entities.Concrete;
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

            

            CreateMap<Request, ReqRequestDto>().ReverseMap();

            CreateMap<Request, RespRequestDto>()
                .ForMember(dest => dest.RequestCreatorName, opt => opt.MapFrom(src => src.Creator.Name))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.Name))
                .ForMember(dest => dest.RequestExecutorName, opt => opt.MapFrom(src => src.Executor != null ? src.Executor.Name : null))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.Name));

        }
    }
}
