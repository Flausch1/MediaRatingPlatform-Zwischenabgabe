using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace HttpServerDemo.Data
{
    internal class MediaRepository : IMediaRepository
    {
        private string connectionString;

        public MediaRepository(string conn)
        {
            connectionString = conn;
        }

        public bool Create(Media media)
        {
            using var connection = new NpgsqlConnection(connectionString);
            connection.Open();

            string sql = """
                INSERT INTO Media
                (Title, Description, MediaType, ReleaseYear, Genres, AgeRestriction, CreatorId)
                VALUES
                (@title, @description, @mediaType, @releaseYear, @genres, @ageRestriction, @creatorId);
                """;

            using var command = new NpgsqlCommand(sql, connection);

            command.Parameters.AddWithValue("@title", media.Title);
            command.Parameters.AddWithValue("@description", media.Description);
            command.Parameters.AddWithValue("@mediaType", media.MediaType);
            command.Parameters.AddWithValue("@releaseYear", media.ReleaseYear);
            command.Parameters.AddWithValue("@genres", JsonSerializer.Serialize(media.Genres));
            command.Parameters.AddWithValue("@ageRestriction", media.AgeRestriction);
            command.Parameters.AddWithValue("@creatorId", media.CreatorId);

            return command.ExecuteNonQuery() > 0;
        }

        public List<Media> GetAll()
        {
            List<Media> mediaList = new List<Media>();

            using var connection = new NpgsqlConnection(connectionString);
            connection.Open();

            string sql = """
                SELECT Id, Title, Description, MediaType, ReleaseYear,
                       Genres, AgeRestriction, CreatorId
                FROM Media;
                """;

            using var command = new NpgsqlCommand(sql, connection);
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                Media media = new Media
                {
                    Id = reader.GetInt32(0),
                    Title = reader.GetString(1),
                    Description = reader.IsDBNull(2) ? "" : reader.GetString(2),
                    MediaType = reader.GetString(3),
                    ReleaseYear = reader.IsDBNull(4) ? 0 : reader.GetInt32(4),
                    Genres = reader.IsDBNull(5)
                        ? new List<string>()
                        : JsonSerializer.Deserialize<List<string>>(reader.GetString(5)) ?? new List<string>(),
                    AgeRestriction = reader.IsDBNull(6) ? 0 : reader.GetInt32(6),
                    CreatorId = reader.GetInt32(7)
                };

                mediaList.Add(media);
            }

            return mediaList;
        }

        public Media? GetById(int id)
        {
            using var connection = new NpgsqlConnection(connectionString);
            connection.Open();

            string sql = """
                SELECT Id, Title, Description, MediaType, ReleaseYear,
                       Genres, AgeRestriction, CreatorId
                FROM Media
                WHERE Id = @id;
                """;

            using var command = new NpgsqlCommand(sql, connection);
            command.Parameters.AddWithValue("@id", id);

            using var reader = command.ExecuteReader();

            if (reader.Read())
            {
                return new Media
                {
                    Id = reader.GetInt32(0),
                    Title = reader.GetString(1),
                    Description = reader.IsDBNull(2) ? "" : reader.GetString(2),
                    MediaType = reader.GetString(3),
                    ReleaseYear = reader.IsDBNull(4) ? 0 : reader.GetInt32(4),
                    Genres = reader.IsDBNull(5)
                        ? new List<string>()
                        : JsonSerializer.Deserialize<List<string>>(reader.GetString(5)) ?? new List<string>(),
                    AgeRestriction = reader.IsDBNull(6) ? 0 : reader.GetInt32(6),
                    CreatorId = reader.GetInt32(7)
                };
            }

            return null;
        }

        public bool Update(Media media)
        {
            using var connection = new NpgsqlConnection(connectionString);
            connection.Open();

            string sql = """
                UPDATE Media
                SET Title = @title,
                    Description = @description,
                    MediaType = @mediaType,
                    ReleaseYear = @releaseYear,
                    Genres = @genres,
                    AgeRestriction = @ageRestriction
                WHERE Id = @id AND CreatorId = @creatorId;
                """;

            using var command = new NpgsqlCommand(sql, connection);

            command.Parameters.AddWithValue("@id", media.Id);
            command.Parameters.AddWithValue("@title", media.Title);
            command.Parameters.AddWithValue("@description", media.Description);
            command.Parameters.AddWithValue("@mediaType", media.MediaType);
            command.Parameters.AddWithValue("@releaseYear", media.ReleaseYear);
            command.Parameters.AddWithValue("@genres", JsonSerializer.Serialize(media.Genres));
            command.Parameters.AddWithValue("@ageRestriction", media.AgeRestriction);
            command.Parameters.AddWithValue("@creatorId", media.CreatorId);

            return command.ExecuteNonQuery() > 0;
        }

        public bool DeleteById(int id, int creatorId)
        {
            using var connection = new NpgsqlConnection(connectionString);
            connection.Open();

            string sql = """
                DELETE FROM Media
                WHERE Id = @id AND CreatorId = @creatorId;
                """;

            using var command = new NpgsqlCommand(sql, connection);

            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@creatorId", creatorId);

            return command.ExecuteNonQuery() > 0;
        }
    }
}