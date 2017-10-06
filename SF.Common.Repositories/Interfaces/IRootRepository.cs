using SF.Common.Root;
using System;

namespace SF.Common.Repositories.Interfaces
{
    public interface IRootRepository
    {
        RootConnection FindConnectionByEmail(string email);

        RootConnection FindConnectionByUserGuid(Guid userGuid);
    }
}