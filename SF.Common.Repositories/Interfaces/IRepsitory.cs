using System;

namespace SF.Common.Repositories.Interfaces
{
    public interface IRepository<T> : IDisposable
    {
        void Save();
    }
}