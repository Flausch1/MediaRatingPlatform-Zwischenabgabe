using System;
using System.Collections.Generic;
using System.Text;




namespace HttpServerDemo.Data
{
    internal interface ITokenRepository
    {
        bool Create(Token token);
        Token? GetByValue(string value);
        bool DeleteByValue(string value);
    }
}
