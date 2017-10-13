using System;

namespace SF.Common.Repositories.Interfaces
{
    public interface IRepository<T> : IDisposable
    {
        #region Public Methods

        void Save();

        #endregion Public Methods
    }
}