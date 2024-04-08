using Microsoft.AspNetCore.Mvc;

namespace robot_controller_api.Controllers;

[ApiController]
[Route("api/maps")]
public class MapsController : ControllerBase
{
    private static readonly List<Map> _maps = new List<Map>
    {
        new Map(1, 10, 10, "Map 1 ", "Description of Map 1"),
        new Map(2, 20, 10, "Map 2 ", "Description of Map 2")
    };

    [HttpGet] // Return all maps as JSON
    public IEnumerable<Map> GetAllMaps()
    {
        return _maps;
    }

    [HttpGet("square")] //Return maps that are square

    public IEnumerable<Map> GetSquareMapsOnly()
    {
        return _maps.Where(map => map.Columns == map.Rows); //Columns and Rows will be same
    }


    [HttpGet("{id}", Name = "GetMap")] // Based on ID, return a map
    public IActionResult GetMapById(int id)
    {
        // Find the map with the specified id
        var map = _maps.FirstOrDefault(m => m.Id == id);

        // If map is not found, return NotFound
        if (map == null)
        {
            return NotFound();
        }
        // If map is found, return Ok with the map object
        return Ok(map);
    }

    [HttpPost] // Add a map to the array
    public IActionResult AddMap(Map newMap)
    {
        if (newMap == null) //A new map value is needed for this method (so check for null value)
        {
            return BadRequest();
        }

        // Check if the map name already exists, if so return with no edits
        if (_maps.Any(m => m.Name == newMap.Name))
        {
            return Conflict();
        }

        // Assign a new unique Id (don't blindly use the one from newMap)
        int newId = _maps.Count + 1;
        newMap.Id = newId;

        // Set CreatedDate and ModifiedDate to DateTime.Now
        newMap.CreatedDate = DateTime.Now;
        newMap.ModifiedDate = DateTime.Now;

        // Add the new map to the list
        _maps.Add(newMap);

        // Return a GET endpoint resource URI (the URI of the added new map)
        return CreatedAtRoute("GetMap", new { id = newMap.Id }, newMap);
    }

    [HttpPut("{id}")] // This endpoint modifies an existing map
    public IActionResult UpdateMap(int id, Map updatedMap)
    {
        // Find the map by id
        var existingMap = _maps.FirstOrDefault(m => m.Id == id);
        // If map with specified id does not exist, return NotFound
        if (existingMap == null)
        {
            return NotFound();
        }

        // Try to update the existing map with details from updatedMap
        try
        {
            existingMap.Columns = updatedMap.Columns;
            existingMap.Rows = updatedMap.Rows;
            existingMap.Name = updatedMap.Name;
            existingMap.Description = updatedMap.Description;
            existingMap.ModifiedDate = DateTime.Now;

            return NoContent();
        }
        catch
        {
            // Return BadRequest if update fails
            return BadRequest();
        }
    }

    [HttpDelete("{id}")] // Delete an existing map
    public IActionResult DeleteMap(int id)
    {
        // Find the map by id
        var mapToRemove = _maps.FirstOrDefault(m => m.Id == id);
        // If map with this id doesn't exist, return NotFound
        if (mapToRemove == null)
        {
            return NotFound();
        }

        // Remove the map from maps
        _maps.Remove(mapToRemove);

        // Return NoContent if deletion was successful
        return NoContent();
    }


    [HttpGet("{id}/{x}-{y}")] //Endpoint 7
    public IActionResult CheckCoordinate(int id, int x, int y)
    {
        // return BadRequest() if coordinate provided is in the wrong format (e.g. negative values)
        if (x < 0 || y < 0)
        {
            return BadRequest("Coordinates must be non-negative values.");
        }

        // Find the map by id
        var map = _maps.FirstOrDefault(m => m.Id == id);

        // If map with input id does not exist, return NotFound
        if (map == null)
        {
            return NotFound();
        }

        bool isOnMap = false;
        // Check whether the coordinate is on the map
        if(x < map.Columns && y < map.Rows){
            isOnMap = true;
        }

        // Return Ok with the boolean result of whether the coordinate is on the map
        return Ok(isOnMap);
    }

}
