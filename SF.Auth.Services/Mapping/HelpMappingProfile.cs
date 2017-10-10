using AutoMapper;
using SF.Auth.DataTransferObjects.Help;
using SF.Common.Help;

namespace SF.Auth.Services.Mapping
{
    internal class HelpMappingProfile : Profile
    {
        public HelpMappingProfile()
            : base()
        {
            CreateMap<HelpLink, HelpLinkDto>()
                .ForMember(d => d.Url, s => s.MapFrom(x => x.Url))
                .ForMember(d => d.LinkText, s => s.MapFrom(x => x.LinkText))
                .ForMember(d => d.Order, s => s.MapFrom(x => x.Order));
        }
    }
}