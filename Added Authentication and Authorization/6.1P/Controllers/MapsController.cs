using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using robot_controller_api.Persistence;


namespace robot_controller_api.Controllers;

[ApiController]
[Route("api/maps")]
public class MapsController : ControllerBase
{
    /// <summary>
    /// Retrieves all maps.
    /// </summary>
    /// <returns>
    /// A collection of all the maps.
    /// </returns>
    /// <response code="200">Returns Ok with the list of maps</response>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [AllowAnonymous]
    [HttpGet] 
    public IEnumerable<Map> GetAllMaps()
    {
        //Return all the maps using the method GetAllMaps() from the class MapDataAccess
        return MapDataAccess.GetAllMaps();
    }

    /// <summary>
    /// Retrieves only square maps.
    /// </summary>
    /// <returns>
    /// A collection of maps that are square.
    /// </returns>
    /// <response code="200">Returns Ok with the list of maps that are square</response>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpGet("square")] 

    public IEnumerable<Map> GetSquareMapsOnly()
    {
        return MapDataAccess.GetSquareMapsOnly();
    }

    /// <summary>
    /// Retrieves a map by its ID.
    /// </summary>
    /// <param name="id">The ID of the map to retrieve.</param>
    /// <returns> The map if map is found.
    /// </returns>
    /// <response code="404">If the Map was not found</response>
    /// <response code="200">Returns Ok with the map object</response>>
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
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

    /// <summary>
    /// Creates a map.
    /// </summary>
    /// <param name="newMap">A new map from the HTTP request.</param>
    /// <returns>A newly created map</returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /api/maps
    ///     {
    ///        "Columns": 5,
    ///        "Rows": 6,
    ///        "Name": "Map 5",
    ///        "Description": " Not Square"
    ///     }
    ///
    /// </remarks>
    /// <response code="201">Returns the newly created map</response>
    /// <response code="400">If the map is null, or addition fails</response>
    /// <response code="409">If a map with the same name already exists.</response>
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [HttpPost]
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


    
    /// <summary>
    /// Updates an existing map.
    /// </summary>
    /// <param name="id">The ID of the map to update.</param>
    /// <param name="updatedMap">The updated map details from the HTTP request.</param>
    /// <returns>NoContent if successful update for Map</returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     PUT /api/maps/:id
    ///     {
    ///        "Columns": 5,
    ///        "Rows": 5,
    ///        "Name": "Map 4"
    ///     }
    ///
    /// </remarks>
    /// <response code="404">If the map isn't found in database</response>
    /// <response code="204">No Content if map is updated sucessfully</response>
    /// <response code="400">If update failed.</response>
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPut("{id}")] 
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

    /// <summary>
    /// Deletes an existing map.
    /// </summary>
    /// <param name="id">The ID of the map to delete.</param>
    /// <returns>NoContent if successful deletion.</returns>
    /// <response code="404">If the map isn't found in database</response>
    /// <response code="204">No Content if Map was deleted</response>
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [Authorize(Policy = "AdminOnly")]
    [HttpDelete("{id}")] 
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

    /// <summary>
    /// // Check whether the coordinate fits in a specific map.
    /// </summary>
    /// <param name="id">The ID of the map that the coordinate is checking it fits in.</param>
    /// <param name="x">The x value of the coordinate.</param>
    /// <param name="y">The y value of the coordinate.</param>
    /// <returns>True if the coordinate fits in the map</returns>
    /// <response code="400">If the coordinate values weren't negative</response>
    /// <response code="404">A map that matches in the input id wasn't found</response>
    /// <response code="200">Returns Ok with the boolean result of whether the coordinate is on the map</response>
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpGet("{id}/{x}-{y}")] 
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
