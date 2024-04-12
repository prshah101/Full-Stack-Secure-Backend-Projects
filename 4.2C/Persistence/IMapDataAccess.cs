namespace robot_controller_api.Persistence
{
    // This intergace defines data access operations for map entities
    public interface IMapDataAccess
    {
        // Retrieve all maps from the data store, so return as a List of maps
        List<Map> GetAllMaps();

        // Retrieve only square maps from the data store, so return as a List of maps
        List<Map> GetSquareMapsOnly();

        // Retrieve a map by its unique identifier, so return a Map object
        Map? GetMapById(int id);

        // Retrieves a map by its name, so return a Map object
        Map? GetMapByName(string name);

        // Adds a new map to the data store, return nothing
        void AddMap(Map newMap);

        // Updates an existing map in the data store, return nothing
        void UpdateMap(int id, Map updatedMap);

        // Deletes a map from the data store by its identifier, return nothing
        void DeleteMap(int id);
    }
}
