namespace robot_controller_api;

public class Map
{
    // Id as Map is an entity, not a value object in DDD terminology
    public int Id { get; set; }

    //A number of columns in the map that will be addressed by X coordinate
    public int Columns { get; set; }

    // A number of rows in the map that will be addressed by Y coordinate.
    public int Rows { get; set; }

    // name of the Map, e.g. LEFT, RIGHT, PLACE.
    public string Name { get; set; }

    // Nullable property to store the description of the map
    public string? Description { get; set; }

    // Sate and time of Map 's creation
    public DateTime CreatedDate { get; set; }

    // Date and time when the Map was last modified.
    public DateTime ModifiedDate { get; set; }

    // Constructor to initialize the map
    public Map(int id, int columns, int rows, string name, string? description = null, DateTime createdDate = default, DateTime modifiedDate = default)
    {
        Id = id;
        Columns = columns;
        Rows = rows;
        Name = name;
        Description = description;
        CreatedDate = DateTime.Now;
        ModifiedDate = DateTime.Now;
    }

}
