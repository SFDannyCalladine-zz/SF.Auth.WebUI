using AutoMapper;
using SF.Auth.Accounts;
using SF.Auth.DataTransferObjects;

namespace SF.Auth.Services.Mapping
{
    public class AuthMappingProfile : Profile
    {
        public AuthMappingProfile()
            : base()
        {
            CreateMap<User, UserDto>()
                .ForMember(d => d.UserId, s => s.MapFrom(x => x.UserId))
                .ForMember(d => d.UserGuid, s => s.MapFrom(x => x.UserGuid))
                .ForMember(d => d.Name, s => s.MapFrom(x => x.Name))
                .ForMember(d => d.Email, s => s.MapFrom(x => x.Email));
        }
    }
}