using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace HttpServerDemo.Data
{
    internal class TestRepo
    {
        private string connectionString;

        public TestRepo(string conn)
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

            var sql = "INSERT INTO Animal (Name, Species, Age, Weight) VALUES (@name, @species, @age, @weight)";

            using var cmd = new NpgsqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("name", animal.Name);
            cmd.Parameters.AddWithValue("species", animal.Species);
            cmd.Parameters.AddWithValue("age", animal.Age);
            cmd.Parameters.AddWithValue("weight", animal.Weight);
            cmd.ExecuteNonQuery();

            return true;
        }

        public bool Update(Animal animal)
        {
            try
            {
                using var connection = GetConnection();

                var sql = "UPDATE Animal SET name = @name, species = @species, age = @age, weight = @weight WHERE id = @id";

                using var cmd = new NpgsqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("name", animal.Name);
                cmd.Parameters.AddWithValue("species", animal.Species);
                cmd.Parameters.AddWithValue("age", animal.Age);
                cmd.Parameters.AddWithValue("weight", animal.Weight);
                cmd.Parameters.AddWithValue("id", animal.Id);

                cmd.ExecuteNonQuery();

                return true;
            }
            catch (PostgresException ex)
            {
                return false;
            }
        }

        public List<Animal> GetAll()
        {
            using var connection = GetConnection();

            var sql = "SELECT * FROM Animal";
            using var cmd = new NpgsqlCommand(sql, connection);
            using var reader = cmd.ExecuteReader();

            var animals = new List<Animal>();

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

        public Animal? GetById(int id)
        {
            using var connection = GetConnection();

            var sql = "SELECT * FROM Animal WHERE id = @id";
            using var cmd = new NpgsqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("id", id);
            using var reader = cmd.ExecuteReader();

            if (!reader.Read())
                return null;

            var animal = new Animal();

            animal.Id = reader.GetInt32(0);
            animal.Name = reader.GetString(1);
            animal.Species = reader.GetString(2);
            animal.Age = reader.GetInt32(3);
            animal.Weight = reader.GetFloat(4);

            return animal;
        }

        public bool DeleteById(int id)
        {
            using var connection = GetConnection();

            var sql = "DELETE FROM Animal WHERE id = @id";
            using var cmd = new NpgsqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("id", id);
            return cmd.ExecuteNonQuery() > 0;
        }

        public void AddWithTransaction()
        {
            // Konto A -100, Konto B + 100

            using var connection = GetConnection();
            using var transaction = connection.BeginTransaction();

            try
            {
                var sql1 = "UPDATE Animal SET weight = 15 WHERE id = 1";
                using var cmd1 = new NpgsqlCommand(sql1, connection, transaction);
                cmd1.ExecuteNonQuery();

                var sql2 = "UPDATE Animal SET weight = 20 WHERE id = 2";
                using var cmd2 = new NpgsqlCommand(sql2, connection, transaction);
                cmd2.ExecuteNonQuery();

                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }
    }
}
