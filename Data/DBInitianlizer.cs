using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;

namespace HttpServerDemo.Data
{
    internal class DBInitianlizer
    {
        private string connectionString;

        public DBInitianlizer(string conn)
        {
            connectionString = conn;
        }

        /*public void DeleteTables()
        {
            try
            {
                using var connection = new NpgsqlConnection(connectionString);

                connection.Open();

                string sql = """
                DROP TABLE IF EXISTS Animal;
                DROP TABLE IF EXISTS Owner;
                DROP TABLE IF EXISTS Food;
                """;

                using var command = new NpgsqlCommand(sql, connection);

                command.ExecuteNonQuery();
            }
            catch (PostgresException ex)
            {
                Console.WriteLine("Connection to database failed: " + ex);
            }
            catch (NpgsqlException  ex)
            {
                Console.WriteLine("Connection to database failed: " + ex);
            }
        }*/

        public void InitDB()
        {
            try
            {
                using var connection = new NpgsqlConnection(connectionString);

                connection.Open();

                string sql = """
                CREATE TABLE IF NOT EXISTS Users (
                    Id SERIAL PRIMARY KEY,
                    Username VARCHAR(100) UNIQUE NOT NULL,
                    Password VARCHAR(255) NOT NULL
                );

                CREATE TABLE IF NOT EXISTS Media (
                    Id SERIAL PRIMARY KEY,
                    Title VARCHAR(200) NOT NULL,
                    Description TEXT,
                    MediaType VARCHAR(20) NOT NULL,
                    ReleaseYear INT,
                    Genres TEXT,
                    AgeRestriction INT,
                    CreatorId INT NOT NULL REFERENCES Users(Id)
                );

                CREATE TABLE IF NOT EXISTS Tokens (
                    Token VARCHAR(255) PRIMARY KEY,
                    UserId INT NOT NULL REFERENCES Users(Id),
                    ExpiresAt TIMESTAMP NOT NULL
                );
                """;

                using var command = new NpgsqlCommand(sql, connection);

                command.ExecuteNonQuery();
            }
            catch (PostgresException ex)
            {
                Console.WriteLine("Connection to database failed: " + ex);
            }
            catch (NpgsqlException ex)
            {
                Console.WriteLine("Connection to database failed: " + ex);
            }
        }

        public void AddTestData()
        {
            using var connection = new NpgsqlConnection(connectionString);
            connection.Open();

            string sql = """
                INSERT INTO Animal (Name, Species, Age, Weight)
                VALUES (@name, @species, @age, @weight);
                """;

            using var command = new NpgsqlCommand(sql, connection);

            command.Parameters.AddWithValue("@name", "Bello");
            command.Parameters.AddWithValue("@species", "Dog");
            command.Parameters.AddWithValue("@age", 5);
            command.Parameters.AddWithValue("@weight", 24.5m);

            int affectedRows = command.ExecuteNonQuery();
        }
    }
}
