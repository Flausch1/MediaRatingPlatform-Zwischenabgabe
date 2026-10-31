using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

using Npgsql;

namespace HttpServerDemo.Data
{
    internal class UserRepository : IUserRepository
    {
        private string connectionString;

        public UserRepository(string conn)
        {
            connectionString = conn;
        }

        public bool Create(User entity)
        {
            using var connection = new NpgsqlConnection(connectionString);
            connection.Open();

            string sql = """
                INSERT INTO Users (Username, Password)
                VALUES (@username, @password);
                """;

            using var command = new NpgsqlCommand(sql, connection);

            command.Parameters.AddWithValue("@username", entity.Username);
            command.Parameters.AddWithValue("@password", entity.Password);

            return command.ExecuteNonQuery() > 0;
        }

        public bool DeleteById(int id)
        {
            using var connection =new NpgsqlConnection(connectionString);
            connection.Open();

            string sql = "DELETE FROM Users WHERE Id = @id";

            using var command = new NpgsqlCommand(sql, connection);
            command.Parameters.AddWithValue("@id", id);

            return command.ExecuteNonQuery() > 0;
        }

        public List<User> GetAll()
        {
            List<User> users = new List<User>();

            using var connection = new NpgsqlConnection(connectionString);
            connection.Open();

            string sql = "SELECT Id, Username, Password FROM Users";

            using var command = new NpgsqlCommand(sql, connection);
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                User user = new User
                {
                    Id = reader.GetInt32(0),
                    Username = reader.GetString(1),
                    Password = reader.GetString(2)
                };

                users.Add(user);
            }

            return users;
        }

        public User? GetById(int id)
        {
            using var connection = new NpgsqlConnection(connectionString);
            connection.Open();

            string sql = """
                SELECT Id, Username, Password
                FROM Users
                WHERE Id = @id;
                """;

           using var command = new NpgsqlCommand(sql, connection);
           command.Parameters.AddWithValue("@id", id);

           using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return new User
                {
                    Id = reader.GetInt32(0),
                    Username = reader.GetString(1),
                    Password = reader.GetString(2)
                };
            }

            return null;
        }

        public User? GetByUsername(string username)
        {
            using var connection = new NpgsqlConnection(connectionString);
            connection.Open();


            string sql = """
                SELECT Id, Username, Password
                FROM Users
                WHERE Username = @username;
                """;

            using var command = new NpgsqlCommand(sql, connection);
            command.Parameters.AddWithValue("@username", username);

            using var reader = command.ExecuteReader();

            if(reader.Read())
            {
                return new User
                {
                    Id = reader.GetInt32(0),
                    Username = reader.GetString(1),
                    Password = reader.GetString(2)
                };
            }

            return null;
        }

        public bool Update(User entity)
        {
            using var connection = new NpgsqlConnection(connectionString);
            connection.Open();

            string sql = """
                UPDATE Users
                SET Username = @username, Password = @password
                WHERE Id = @id;
                """;

            using var command = new NpgsqlCommand(sql, connection);

            command.Parameters.AddWithValue("@id", entity.Id);
            command.Parameters.AddWithValue("@username", entity.Username);
            command.Parameters.AddWithValue("@password", entity.Password);

            return command.ExecuteNonQuery() > 0;
        }
    }
}
