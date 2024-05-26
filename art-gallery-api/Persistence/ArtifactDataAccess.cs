using System;
using System.Collections.Generic;
using Npgsql;

namespace art_gallery_api.Persistence
{
    public static class ArtifactDataAccess
    {
        private const string CONNECTION_STRING = "Host=localhost;Username=postgres;Password=password;Database=diy";

        
        public static List<Artifact> GetAllArtifacts()
        {
            var artifacts = new List<Artifact>();

            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();

            using var cmd = new NpgsqlCommand("SELECT * FROM artifacts", conn);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                artifacts.Add(new Artifact(
                    id: Convert.ToInt32(reader["id"]),
                    name: Convert.ToString(reader["name"]),
                    description: reader["description"] is DBNull ? null : Convert.ToString(reader["description"]),
                    artist: reader["artist"] is DBNull ? null : Convert.ToString(reader["artist"]),
                    category: reader["category"] is DBNull ? null : Convert.ToString(reader["category"]),
                    createdDate: Convert.ToDateTime(reader["created_date"]),
                    modifiedDate: Convert.ToDateTime(reader["modified_date"])
                ));
            }

            return artifacts;
        }

        public static Artifact GetArtifactById(int id)
        {
            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();

            using var cmd = new NpgsqlCommand("SELECT * FROM artifacts WHERE id = @Id", conn);
            cmd.Parameters.AddWithValue("@Id", id);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new Artifact(
                    id: Convert.ToInt32(reader["id"]),
                    name: Convert.ToString(reader["name"]),
                    description: reader["description"] is DBNull ? null : Convert.ToString(reader["description"]),
                    artist: reader["artist"] is DBNull ? null : Convert.ToString(reader["artist"]),
                    category: reader["category"] is DBNull ? null : Convert.ToString(reader["category"]),
                    createdDate: Convert.ToDateTime(reader["created_date"]),
                    modifiedDate: Convert.ToDateTime(reader["modified_date"])
                );
            }
            else
            {
                return null;
            }
        }

        public static Artifact GetArtifactByName(string Name)
        {
            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();

            using var cmd = new NpgsqlCommand("SELECT * FROM artifacts WHERE name = @Name", conn);
            cmd.Parameters.AddWithValue("@Name", Name);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new Artifact(
                    id: Convert.ToInt32(reader["id"]),
                    name: Convert.ToString(reader["name"]),
                    description: reader["description"] is DBNull ? null : Convert.ToString(reader["description"]),
                    artist: reader["artist"] is DBNull ? null : Convert.ToString(reader["artist"]),
                    category: reader["category"] is DBNull ? null : Convert.ToString(reader["category"]),
                    createdDate: Convert.ToDateTime(reader["created_date"]),
                    modifiedDate: Convert.ToDateTime(reader["modified_date"])
                );
            }
            else
            {
                return null;
            }
        }

        public static void AddArtifact(Artifact newArtifact)
        {
            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();

            using var cmd = new NpgsqlCommand("INSERT INTO artifacts (name, description, artist, category, created_date, modified_date) " +
                                              "VALUES (@Name, @Description, @Artist, @Category, @CreatedDate, @ModifiedDate)", conn);
            cmd.Parameters.AddWithValue("@Name", newArtifact.Name);
            cmd.Parameters.AddWithValue("@Description", newArtifact.Description);
            cmd.Parameters.AddWithValue("@Artist", newArtifact.Artist);
            cmd.Parameters.AddWithValue("@Category", newArtifact.Category);
            cmd.Parameters.AddWithValue("@CreatedDate", newArtifact.CreatedDate);
            cmd.Parameters.AddWithValue("@ModifiedDate", newArtifact.ModifiedDate);

            cmd.ExecuteNonQuery();
        }


        public static void UpdateArtifact(int id, Artifact updatedArtifact)
        {
            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();

            using var cmd = new NpgsqlCommand("UPDATE artifacts SET name = @Name, description = @Description, artist = @Artist, " +
                                              "category = @Category, modified_date = @ModifiedDate WHERE id = @Id", conn);
            cmd.Parameters.AddWithValue("@Id", id);
            cmd.Parameters.AddWithValue("@Name", updatedArtifact.Name);
            cmd.Parameters.AddWithValue("@Description", updatedArtifact.Description);
            cmd.Parameters.AddWithValue("@Artist", updatedArtifact.Artist);
            cmd.Parameters.AddWithValue("@Category", updatedArtifact.Category);
            cmd.Parameters.AddWithValue("@ModifiedDate", updatedArtifact.ModifiedDate);

            cmd.ExecuteNonQuery();
        }

        public static void DeleteArtifact(int id)
        {
            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();

            using var cmd = new NpgsqlCommand("DELETE FROM artifacts WHERE id = @Id", conn);
            cmd.Parameters.AddWithValue("@Id", id);

            cmd.ExecuteNonQuery();
        }
    }
}
