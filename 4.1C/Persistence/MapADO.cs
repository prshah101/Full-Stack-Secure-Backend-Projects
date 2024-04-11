using Npgsql;
using System;
using System.Collections.Generic;

namespace robot_controller_api.Persistence
{
    public class MapADO: IMapDataAccess
    {
        // Connection string for connecting to the database
        private const string CONNECTION_STRING = "Host=localhost;Username=postgres;Password=password;Database=sit331";

        // Gets all maps from the database
        public List<Map> GetAllMaps()
        {
            var maps = new List<Map>();
            // Establish a connection to the database
            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();
            // Execute an SQL command to select all maps
            using var cmd = new NpgsqlCommand("SELECT * FROM map", conn);
            // Executes the command and retrieves data from database
            using var dr = cmd.ExecuteReader();
            // Reads each row of the result
            while (dr.Read())
            {
                // Retrieve values from the data reader. Right hand side are the values store in the Server, LHS are how they will be refered to in this class.
                int id = (int)dr["id"];
                int columns = (int)dr["columns"];
                int rows = (int)dr["rows"];
                string name = (string)dr["name"];
                string? description = dr["description"] as string;
                DateTime createdDate = (DateTime)dr["createddate"];
                DateTime modifiedDate = (DateTime)dr["modifieddate"];

                // Create a Map object and add it to the list
                Map map = new Map(id, columns, rows, name, description, createdDate, modifiedDate);
                maps.Add(map);
            }
            return maps;
        }

        // Retrieve square maps only (where columns equal rows) from the database
        public List<Map> GetSquareMapsOnly()
        {
            var maps = new List<Map>();
            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();
            // Execute SQL command to select square maps
            using var cmd = new NpgsqlCommand("SELECT * FROM map WHERE columns = rows", conn); 
            using var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                int id = (int)dr["id"];
                int columns = (int)dr["columns"];
                int rows = (int)dr["rows"];
                string name = (string)dr["name"];
                string? description = dr["description"] as string;
                DateTime createdDate = (DateTime)dr["createddate"];
                DateTime modifiedDate = (DateTime)dr["modifieddate"];

                Map map = new Map(id, columns, rows, name, description, createdDate, modifiedDate);
                maps.Add(map);
            }
            return maps;
        }

        // Retrieve a map by its ID from the database
        public Map GetMapById(int id)
        {
            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();
            // Execute SQL command to select the map at a specific id
            using var cmd = new NpgsqlCommand("SELECT * FROM map WHERE id = @Id", conn);
            cmd.Parameters.AddWithValue("@Id", id);
            using var dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                int columns = (int)dr["columns"];
                int rows = (int)dr["rows"];
                string name = (string)dr["name"];
                string? description = dr["description"] as string;
                DateTime createdDate = (DateTime)dr["createddate"];
                DateTime modifiedDate = (DateTime)dr["modifieddate"];

                return new Map(id, columns, rows, name, description, createdDate, modifiedDate);
            }
            return null;
        }

        // Retrieve a map by its name from the database
        public Map GetMapByName(string Name)
        {
            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();
            using var cmd = new NpgsqlCommand("SELECT * FROM map WHERE \"name\" LIKE @NAME", conn);
            cmd.Parameters.AddWithValue("@name", Name);
            using var dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                int id = (int)dr["id"];
                int columns = (int)dr["columns"];
                int rows = (int)dr["rows"];
                string name = (string)dr["name"];
                string? description = dr["description"] as string;
                DateTime createdDate = (DateTime)dr["createddate"];
                DateTime modifiedDate = (DateTime)dr["modifieddate"];

                return new Map(id, columns, rows, name, description, createdDate, modifiedDate);
            }
            return null;
        }


        // Update an existing map in the database
        public void UpdateMap(int id, Map updatedMap)
        {
            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();
            using var cmd = new NpgsqlCommand("UPDATE map SET columns = @Columns, rows = @Rows, name = @Name, modifieddate = @ModifiedDate WHERE id = @Id", conn);
            cmd.Parameters.AddWithValue("@Id", id);
            cmd.Parameters.AddWithValue("@Columns", updatedMap.Columns);
            cmd.Parameters.AddWithValue("@Rows", updatedMap.Rows);
            cmd.Parameters.AddWithValue("@Name", updatedMap.Name);
            cmd.Parameters.AddWithValue("@ModifiedDate", DateTime.Now);
            cmd.ExecuteNonQuery();
        }

        // Add a new map to the database
        public void AddMap(Map newMap)
        {
            int newId;
            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();

            // Retrieve the highest ID currently to create the next ID in the database
            using (var getMaxIdCmd = new NpgsqlCommand("SELECT MAX(id) FROM map", conn))
            {
                object maxIdObj = getMaxIdCmd.ExecuteScalar();
                // If maxIdObj == DBNull.Value is true, maxId  = 0. Else, maxId  = maxIdObj as an integer
                int maxId = maxIdObj == DBNull.Value ? 0 : Convert.ToInt32(maxIdObj);
                newId = maxId + 1;
            }

            using var cmd = new NpgsqlCommand("INSERT INTO map (columns, rows, name, description, createddate, modifieddate) VALUES (@Columns, @Rows, @Name, @Description, @CreatedDate, @ModifiedDate)", conn);
            cmd.Parameters.AddWithValue("@Columns", newMap.Columns);
            cmd.Parameters.AddWithValue("@Rows", newMap.Rows);
            cmd.Parameters.AddWithValue("@Name", newMap.Name);
            cmd.Parameters.AddWithValue("@Description", newMap.Description);
            cmd.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
            cmd.Parameters.AddWithValue("@ModifiedDate", DateTime.Now);
            cmd.ExecuteNonQuery();
        }

        // Delete a map from the database by its ID
        public void DeleteMap(int id)
        {
            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();
            using var cmd = new NpgsqlCommand("DELETE FROM map WHERE id = @Id", conn);
            cmd.Parameters.AddWithValue("@Id", id);
            cmd.ExecuteNonQuery();
        }
    }
}
