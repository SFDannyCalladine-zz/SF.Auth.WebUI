using System.Collections.Generic;
using SF.Auth.DataTransferObjects.Help;
using SF.Common.ServiceModels.Response;

namespace SF.Auth.Services.Interfaces
{
    public interface IHelpService
    {
        #region Public Methods

        Response<IList<HelpLinkDto>> GetAllLinks();

        #endregion Public Methods
    }
}