 using System;
using System.Collections.Generic;
using System.Text;


using Npgsql;

namespace HttpServerDemo.Data
{
    internal class TokenRepository : ITokenRepository
    {
        private string connectionString;

        public TokenRepository(string conn)
        {
            connectionString = conn;
        }

        public bool Create(Token token)
        {
            using var connection = new NpgsqlConnection(connectionString);
            connection.Open();

            string sql = """
                INSERT INTO Tokens (Token, UserId, ExpiresAt)
                VALUES (@token, @userId, @expiresAt);
                """;

            using var command = new NpgsqlCommand(sql, connection);

            command.Parameters.AddWithValue("@token", token.Value);
            command.Parameters.AddWithValue("@userId", token.UserId);
            command.Parameters.AddWithValue("@expiresAt", token.ExpiresAt);

            return command.ExecuteNonQuery() > 0;
        }

        public Token? GetByValue(string value)
        {
            using var connection = new NpgsqlConnection(connectionString);
            connection.Open();

            string sql = """
                SELECT Token, UserId, ExpiresAt
                FROM Tokens
                WHERE Token = @token;
                """;

            using var command = new NpgsqlCommand(sql, connection);
            command.Parameters.AddWithValue("@token", value);

            using var reader = command.ExecuteReader();

            if (reader.Read())
            {
                return new Token
                {
                    Value = reader.GetString(0),
                    UserId = reader.GetInt32(1),
                    ExpiresAt = reader.GetDateTime(2)
                };
            }

            return null;
        }

        public bool DeleteByValue(string value)
        {
            using var connection = new NpgsqlConnection(connectionString);
            connection.Open();

            string sql = "DELETE FROM Tokens WHERE Token = @token";

            using var command = new NpgsqlCommand(sql, connection);
            command.Parameters.AddWithValue("@token", value);

            return command.ExecuteNonQuery() > 0;
        }
    }
}


