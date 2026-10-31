using System;
using System.Collections.Generic;
using System.Text;

namespace HttpServerDemo.Data
{
    internal interface IRepository<T>
    {
        bool Create(T entity);
        bool Update(T entity);
        bool DeleteById(int id);
        T? GetById(int id);
        List<T> GetAll();
    }
}
