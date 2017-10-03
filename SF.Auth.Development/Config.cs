using IdentityServer4.Test;
using System.Collections.Generic;

namespace SF.Auth.Development
{
    public class Config
    {
        public static List<TestUser> GetUsers()
        {
            return new List<TestUser>
        {
        new TestUser
        {
            SubjectId = "1",
            Username = "alice",
            Password = "password"
        },
        new TestUser
        {
            SubjectId = "2",
            Username = "bob",
            Password = "password"
        }
    };
        }
    }
}