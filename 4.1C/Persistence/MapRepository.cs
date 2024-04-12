using Npgsql;
using System;
using System.Collections.Generic;

namespace robot_controller_api.Persistence
{
    // This class implements data access operations for maps using ADO.NET
    public class MapRepository : IMapDataAccess, IRepository
    {
        private IRepository _repo => this;

        // Method to retrieve all maps from the database
        public List<Map> GetAllMaps()
        {
            var maps = _repo.ExecuteReader<Map>("SELECT * FROM public.map");
            return maps;
        }

        // Method to retrieve square maps only (where columns equal rows) from the database
        public List<Map> GetSquareMapsOnly()
        {
            return _repo.ExecuteReader<Map>("SELECT * FROM map WHERE columns = rows");
        }

        // Method to retrieve a map from the database, based on its ID
        public Map? GetMapById(int id)
        {
            var sqlParams = new NpgsqlParameter[]
            {
                new NpgsqlParameter("@Id", id)
            };

            return _repo.ExecuteReader<Map>(
                "SELECT * FROM map WHERE id = @Id",
                sqlParams
            ).FirstOrDefault();
        }

        // Method to retrieve a map from the database, based on its name 
        public Map? GetMapByName(string name)
        {
            var sqlParams = new NpgsqlParameter[]
            {
        new NpgsqlParameter("@Name", name)
            };

            return _repo.ExecuteReader<Map>(
                "SELECT * FROM map WHERE \"name\" LIKE @Name",
                sqlParams
            ).FirstOrDefault();
        }
        
        // Method to add a new map to the database
        public void AddMap(Map newMap)
        {
            var sqlParams = new NpgsqlParameter[]
            {
                new NpgsqlParameter("Columns", newMap.Columns),
                new NpgsqlParameter("Rows", newMap.Rows),
                new NpgsqlParameter("Name", newMap.Name),
                new NpgsqlParameter("Description", newMap.Description ?? (object)DBNull.Value),
                new NpgsqlParameter("CreatedDate", DateTime.Now),
                new NpgsqlParameter("ModifiedDate", DateTime.Now)
            };

            _repo.ExecuteReader<Map>(
                "INSERT INTO map (columns, rows, name, description, createddate, modifieddate) VALUES (@Columns, @Rows, @Name, @Description, @CreatedDate, @ModifiedDate);",
                sqlParams
            );
        }

        // Method to update an existing map in the database
        public void UpdateMap(int id, Map updatedMap)
        {
            var sqlParams = new NpgsqlParameter[]
            {
                new NpgsqlParameter("Id", id),
                new NpgsqlParameter("Columns", updatedMap.Columns),
                new NpgsqlParameter("Rows", updatedMap.Rows),
                new NpgsqlParameter("Name", updatedMap.Name),
                new NpgsqlParameter("Description", updatedMap.Description ?? (object)DBNull.Value),
                new NpgsqlParameter("ModifiedDate", DateTime.Now)
            };

            _repo.ExecuteReader<Map>(
                "UPDATE map SET columns = @Columns, rows = @Rows, name = @Name, description = @Description, modifieddate = @ModifiedDate WHERE id = @Id;",
                sqlParams
            );
        }

        // Method to delete a map from the database by its ID
        public void DeleteMap(int id)
        {
            var sqlParams = new NpgsqlParameter[]
            {
                new NpgsqlParameter("Id", id)
            };

            _repo.ExecuteReader<Map>(
                "DELETE FROM map WHERE id = @Id;",
                sqlParams
            );
        }

    }
}
