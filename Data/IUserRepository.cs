using System;
using System.Collections.Generic;
using System.Text;

namespace HttpServerDemo.Data
{
    internal interface IUserRepository : IRepository<User>
    {
        User? GetByUsername(string username);
    }
}
