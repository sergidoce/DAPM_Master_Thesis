using AutoMapper;
using DAPM.Authenticator.Models;
using DAPM.Authenticator.Models.Dto;
namespace DAPM.Authenticator.Util
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<RegistrationDto, User>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName));

            CreateMap<User, UserDto>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.OrganizationName, opt => opt.MapFrom(src => src.OrganizationName));

        }
    }
}
