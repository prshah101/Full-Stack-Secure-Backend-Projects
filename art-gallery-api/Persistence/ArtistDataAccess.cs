using System.Collections.Generic;
using Npgsql;

namespace art_gallery_api.Persistence
{
    public static class ArtistDataAccess
    {
        private const string CONNECTION_STRING = "Host=localhost;Username=postgres;Password=password;Database=diy";

        public static IEnumerable<Artist> GetAllArtists()
        {
            var artists = new List<Artist>();
            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();
            using var cmd = new NpgsqlCommand("SELECT * FROM artists", conn);
            using var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                var artist = new Artist(
                    id: (int)dr["id"],
                    name: (string)dr["name"],
                    genre: dr["genre"] as string,
                    biography: dr["biography"] as string,
                    country: dr["country"] as string,
                    createdDate: (DateTime)dr["created_date"],
                    modifiedDate: (DateTime)dr["modified_date"]
                );
                artists.Add(artist);
            }
            return artists;
        }

        public static Artist GetArtistById(int id)
        {
            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();
            using var cmd = new NpgsqlCommand("SELECT * FROM artists WHERE id = @Id", conn);
            cmd.Parameters.AddWithValue("@Id", id);
            using var dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                return new Artist(
                    id: (int)dr["id"],
                    name: (string)dr["name"],
                    genre: dr["genre"] as string,
                    biography: dr["biography"] as string,
                    country: dr["country"] as string,
                    createdDate: (DateTime)dr["created_date"],
                    modifiedDate: (DateTime)dr["modified_date"]
                );
            }
            return null;
        }

        public static Artist GetArtistByName(string Name)
        {
            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();
            using var cmd = new NpgsqlCommand("SELECT * FROM artists WHERE name = @Name", conn);
            cmd.Parameters.AddWithValue("@Name", Name);
            using var dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                return new Artist(
                    id: (int)dr["id"],
                    name: (string)dr["name"],
                    genre: dr["genre"] as string,
                    biography: dr["biography"] as string,
                    country: dr["country"] as string,
                    createdDate: (DateTime)dr["created_date"],
                    modifiedDate: (DateTime)dr["modified_date"]
                );
            }
            return null;
        }

        public static void AddArtist(Artist newArtist)
        {
            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();
            using var cmd = new NpgsqlCommand(
                "INSERT INTO artists (name, genre, biography, country, created_date, modified_date) VALUES (@Name, @Genre, @Biography, @Country, @CreatedDate, @ModifiedDate)", conn);
            cmd.Parameters.AddWithValue("@Name", newArtist.Name);
            cmd.Parameters.AddWithValue("@Genre", newArtist.Genre);
            cmd.Parameters.AddWithValue("@Biography", newArtist.Biography);
            cmd.Parameters.AddWithValue("@Country", newArtist.Country);
            cmd.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
            cmd.Parameters.AddWithValue("@ModifiedDate", DateTime.Now);
            cmd.ExecuteNonQuery();
        }

        public static void UpdateArtist(int id, Artist updatedArtist)
        {
            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();
            using var cmd = new NpgsqlCommand(
                "UPDATE artists SET name = @Name, genre = @Genre, biography = @Biography, country = @Country, modified_date = @ModifiedDate WHERE id = @Id", conn);
            cmd.Parameters.AddWithValue("@Id", id);
            cmd.Parameters.AddWithValue("@Name", updatedArtist.Name);
            cmd.Parameters.AddWithValue("@Genre", updatedArtist.Genre);
            cmd.Parameters.AddWithValue("@Biography", updatedArtist.Biography);
            cmd.Parameters.AddWithValue("@Country", updatedArtist.Country);
            cmd.Parameters.AddWithValue("@ModifiedDate", DateTime.Now);
            cmd.ExecuteNonQuery();
        }

        public static void DeleteArtist(int id)
        {
            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();
            using var cmd = new NpgsqlCommand("DELETE FROM artists WHERE id = @Id", conn);
            cmd.Parameters.AddWithValue("@Id", id);
            cmd.ExecuteNonQuery();
        }
    }
}
