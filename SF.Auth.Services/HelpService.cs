using AutoMapper;
using SF.Auth.DataTransferObjects.Help;
using SF.Auth.Repositories.Interfaces;
using SF.Auth.Services.Interfaces;
using SF.Common.ServiceModels.Response;
using SF.Common.Services;
using System;
using System.Collections.Generic;

namespace SF.Auth.Services
{
    public class HelpService : BaseService, IHelpService
    {
        private readonly IHelpRepository _helpRepository;

        private readonly IMapper _mapper;

        public HelpService(
            IHelpRepository helpRepository,
            IMapper mapper)
        {
            _helpRepository = helpRepository;

            _mapper = mapper;
        }

        public Response<IList<HelpLinkDto>> GetAllLinks()
        {
            try
            {
                var helpLinks = _helpRepository.GetAllLinks();

                var dtos = _mapper.Map<IList<HelpLinkDto>>(helpLinks);

                return new Response<IList<HelpLinkDto>>(dtos);
            }
            catch (Exception e)
            {
                return HandleException<IList<HelpLinkDto>>(e);
            }
        }
    }
}