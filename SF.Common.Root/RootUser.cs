using System;

namespace SF.Common.Root
{
    public class RootUser
    {
        public Guid AccountGuid { get; private set; }

        public string Email { get; private set; }

        public Guid UserGuid { get; private set; }

        private RootUser()
        {
        }
    }
}