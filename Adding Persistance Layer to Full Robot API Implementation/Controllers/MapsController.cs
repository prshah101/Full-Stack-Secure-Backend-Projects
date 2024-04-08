using Microsoft.AspNetCore.Mvc;
using robot_controller_api.Persistence;


namespace robot_controller_api.Controllers;

[ApiController]
[Route("api/maps")]
public class MapsController : ControllerBase
{
    [HttpGet] // Return all maps as JSON
    public IEnumerable<Map> GetAllMaps()
    {
        //Return all the maps using the method GetAllMaps() from the class MapDataAccess
        return MapDataAccess.GetAllMaps();
    }

    [HttpGet("square")] //Return maps that are square

    public IEnumerable<Map> GetSquareMapsOnly()
    {
        return MapDataAccess.GetSquareMapsOnly();
    }


    [HttpGet("{id}", Name = "GetMap")] // Based on ID, return a map
    public IActionResult GetMapById(int id)
    {
        // Retrieve the map with the specified id
        Map map = MapDataAccess.GetMapById(id);

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
        Map? mapExists = MapDataAccess.GetMapByName(newMap.Name);
        if (mapExists != null && mapExists.Name == newMap.Name)
        {
            return Conflict();
        }

        try
        {
            //Try to add the map command to the database
            MapDataAccess.AddMap(newMap);
            Map? addedNewMap = MapDataAccess.GetMapByName(newMap.Name);
            // Return a GET endpoint resource URI (the URI of the added new map)
            return CreatedAtRoute("GetRobotCommand", new { id = addedNewMap.Id }, addedNewMap);
        }
        catch
        {
            // Return BadRequest if addition fails
            return BadRequest();
        }
    }

    [HttpPut("{id}")] // This endpoint modifies an existing map
    public IActionResult UpdateMap(int id, Map updatedMap)
    {
        // Find the map by id
        Map existingMap = MapDataAccess.GetMapById(id);

        // If map with specified id does not exist, return NotFound
        if (existingMap == null)
        {
            return NotFound();
        }

        // Try to update the existing map with details from updatedMap
        try
        {
            MapDataAccess.UpdateMap(id, updatedMap);

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
        Map map = MapDataAccess.GetMapById(id);

        // If map with this id doesn't exist, return NotFound
        if (map == null)
        {
            return NotFound();
        }

        // Remove the map from maps
        MapDataAccess.DeleteMap(id);

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
        Map map = MapDataAccess.GetMapById(id);

        // If map with input id does not exist, return NotFound
        if (map == null)
        {
            return NotFound();
        }

        bool isOnMap = false;
        // Check whether the coordinate is on the map
        if (x < map.Columns && y < map.Rows)
        {
            isOnMap = true;
        }

        // Return Ok with the boolean result of whether the coordinate is on the map
        return Ok(isOnMap);
    }

}
