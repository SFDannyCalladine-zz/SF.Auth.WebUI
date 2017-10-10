using SF.Common.Help;
using System.Collections.Generic;

namespace SF.Auth.Repositories.Interfaces
{
    public interface IHelpRepository
    {
        IList<HelpLink> GetAllLinks();
    }
}