namespace robot_controller_api.Persistence
{
    public interface IMapDataAccess
    {
        List<Map> GetAllMaps();
        List<Map> GetSquareMapsOnly();
        Map GetMapById(int id);
        Map GetMapByName(string name);
        void AddMap(Map newMap);
        void UpdateMap(int id, Map updatedMap);
        void DeleteMap(int id);
    }
}
