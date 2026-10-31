using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;

namespace HttpServerDemo.Data
{
    internal class AnimalPostgresRepository : IAnimalRepository
    {
        private string connectionString;

        public AnimalPostgresRepository(string conn)
        {
            connectionString = conn;
        }

        private NpgsqlConnection GetConnection()
        {
            var connection = new NpgsqlConnection(connectionString);

            connection.Open();

            return connection;
        }
        public bool Create(Animal animal)
        {
            using var connection = GetConnection();

            var sql = "INSERT INTO Animal (name, species, age, weight) VALUES (@name, @species, @age, @weight)";

            using var cmd = new NpgsqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("name", animal.Name);
            cmd.Parameters.AddWithValue("species", animal.Species);
            cmd.Parameters.AddWithValue("age", animal.Age);
            cmd.Parameters.AddWithValue("weight", animal.Weight);
            return cmd.ExecuteNonQuery() > 0;
        }

        public bool Update(Animal animal)
        {
            using var connection = GetConnection();

            var sql = "UPDATE Animal SET name = @name, species = @species, age = @age, weight = @weight WHERE id = @id";

            using var cmd = new NpgsqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("name", animal.Name);
            cmd.Parameters.AddWithValue("species", animal.Species);
            cmd.Parameters.AddWithValue("age", animal.Age);
            cmd.Parameters.AddWithValue("weight", animal.Weight);
            cmd.Parameters.AddWithValue("id", animal.Id);
            return cmd.ExecuteNonQuery() > 0;
        }

        public bool DeleteById(int id)
        {
            using var connection = new NpgsqlConnection(connectionString);

            connection.Open();
            var sql = "DELETE FROM Animal WHERE id = @id";
            using var cmd = new NpgsqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("id", id);
            return cmd.ExecuteNonQuery() > 0;
        }

        public Animal? GetById(int id)
        {
            using var connection = GetConnection();

            var sql = "SELECT * FROM Animal WHERE id = @id";

            using var cmd = new NpgsqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("@id", id);
            using var reader = cmd.ExecuteReader();

            if (!reader.Read())
                return null;

            Animal animal = new Animal();
            animal.Id = reader.GetInt32(0);
            animal.Name = reader.GetString(1);
            animal.Species = reader.GetString(2);
            animal.Age = reader.GetInt32(3);
            animal.Weight = reader.GetFloat(4);

            return animal;
        }

        public List<Animal> GetAll()
        {
            using var connection = GetConnection();

            var sql = "SELECT * FROM Animal";

            using var cmd = new NpgsqlCommand(sql, connection);
            using var reader = cmd.ExecuteReader();

            List<Animal> animals = new List<Animal>();

            while (reader.Read())
            {
                Animal animal = new Animal();
                animal.Id = reader.GetInt32(0);
                animal.Name = reader.GetString(1);
                animal.Species = reader.GetString(2);
                animal.Age = reader.GetInt32(3);
                animal.Weight = reader.GetFloat(4);

                animals.Add(animal);
            }

            return animals;
        }
    }
}
