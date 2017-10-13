using System.Collections.Generic;
using SF.Common.Help;

namespace SF.Auth.Repositories.Interfaces
{
    public interface IHelpRepository
    {
        #region Public Methods

        IList<HelpLink> GetAllLinks();

        #endregion Public Methods
    }
}