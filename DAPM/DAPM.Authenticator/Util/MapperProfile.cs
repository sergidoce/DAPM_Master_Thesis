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
        }
    }
}
