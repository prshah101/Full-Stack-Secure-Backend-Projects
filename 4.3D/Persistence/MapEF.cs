using Npgsql;
using System;
using robot_controller_api.Models;
using System.Collections.Generic;

namespace robot_controller_api.Persistence
{
    // This class implements data access operations for maps using ADO.NET
    public class MapEF : RobotContext, IMapDataAccess
    {
        private readonly RobotContext _robotContext;

        public MapEF(RobotContext robotContext)
        {
            _robotContext = robotContext;
        }

        // Method to retrieve all maps from the database
        public List<Map> GetAllMaps()
        {
             return _robotContext.Maps.OrderBy(x=>x.Id).ToList();
        }

        // Method to retrieve square maps only (where columns equal rows) from the database
        public List<Map> GetSquareMapsOnly()
        {
            var maps = _robotContext.Maps
                .Where(x=>x.Columns == x.Rows)
                .ToList();
            return maps;
        }

        // Method to retrieve a map from the database, based on its ID
        public Map? GetMapById(int id)
        {
            return (Map?)_robotContext.Maps.FirstOrDefault(x=>x.Id == id);
        }

        // Method to retrieve a map from the database, based on its name 
        public Map? GetMapByName(string name)
        {
            return (Map?)_robotContext.Maps.FirstOrDefault(x=>x.Name == name);
        }
        
        // Method to add a new map to the database
        public void AddMap(Map newMap)
        {
            _robotContext.Maps.Add(newMap);
            _robotContext.SaveChanges();
            
        }

        // Method to update an existing map in the database
        public void UpdateMap(int id, Map updatedMap)
        {

            // Find the existing map by its ID
            var existingMap = _robotContext.Maps.FirstOrDefault(c => c.Id == id);
            
            // If the map with the given ID is found
            if (existingMap != null)
            {
                // Update the properties of the existing map
                existingMap.Name = updatedMap.Name;
                existingMap.Description = updatedMap.Description;
                existingMap.Columns = updatedMap.Columns;
                existingMap.Rows = updatedMap.Rows;
                existingMap.ModifiedDate = DateTime.Now; 
                
                // Save changes to the database
                _robotContext.SaveChanges();
            }
        }

        // Method to delete a map from the database by its ID
        public void DeleteMap(int id)
        {
            // Find the existing map by its ID
            var mapToDelete = _robotContext.Maps.FirstOrDefault(c => c.Id == id);

            // If the Map with the given ID is found
            if (mapToDelete != null)
            {
                // Remove the map from the DbSet in the context
                _robotContext.Maps.Remove(mapToDelete);

                // Save changes to the database
                _robotContext.SaveChanges();
            }
        }

    }
}
