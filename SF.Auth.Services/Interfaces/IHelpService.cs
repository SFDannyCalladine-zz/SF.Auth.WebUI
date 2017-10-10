using SF.Auth.DataTransferObjects.Help;
using SF.Common.ServiceModels.Response;
using System.Collections.Generic;

namespace SF.Auth.Services.Interfaces
{
    public interface IHelpService
    {
        Response<IList<HelpLinkDto>> GetAllLinks();
    }
}