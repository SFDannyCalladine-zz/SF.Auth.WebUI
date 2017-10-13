using System;
using System.Collections.Generic;
using AutoMapper;
using SF.Auth.DataTransferObjects.Help;
using SF.Auth.Repositories.Interfaces;
using SF.Auth.Services.Interfaces;
using SF.Common.ServiceModels.Response;
using SF.Common.Services;

namespace SF.Auth.Services
{
    public class HelpService : BaseService, IHelpService
    {
        #region Private Fields

        private readonly IHelpRepository _helpRepository;

        private readonly IMapper _mapper;

        #endregion Private Fields

        #region Public Constructors

        public HelpService(
            IHelpRepository helpRepository,
            IMapper mapper)
        {
            _helpRepository = helpRepository;

            _mapper = mapper;
        }

        #endregion Public Constructors

        #region Public Methods

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

        #endregion Public Methods
    }
}