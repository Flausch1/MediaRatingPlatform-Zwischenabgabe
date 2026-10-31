using System;
using System.Collections.Generic;
using System.Text;

namespace HttpServerDemo.Data
{
    internal class AnimalSQLiteRepositroy : IAnimalRepository
    {
        private string connectionString;

        public AnimalSQLiteRepositroy(string conn)
        {
            connectionString = conn;
        }

        public bool Create(Animal entity)
        {
            throw new NotImplementedException();
        }

        public bool DeleteById(int id)
        {
            throw new NotImplementedException();
        }

        public List<Animal> GetAll()
        {
            throw new NotImplementedException();
        }

        public Animal? GetById(int id)
        {
            throw new NotImplementedException();
        }

        public bool Update(Animal entity)
        {
            throw new NotImplementedException();
        }
    }
}
