using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using robot_controller_api.Persistence;

namespace robot_controller_api.Controllers;

[ApiController]//5
[Route("api/robot-commands")]
public class RobotCommandsController : ControllerBase
{
    /// <summary>
    /// Retrieves all robot commands.
    /// </summary>
    /// <returns>
    /// A collection of all robot commands.
    /// </returns>
    /// <response code="200">Returns Ok with the list of robot commands</response>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [AllowAnonymous]
    [HttpGet("")]
    public IEnumerable<RobotCommand> GetAllRobotCommands()
    {
        //Return all the robot commands using the method GetRobotCommands() from the class RobotCommandDataAccess
        return RobotCommandDataAccess.GetRobotCommands();
    }

    /// <summary>
    /// Retrieves only robot commands that move the robot.
    /// </summary>
    /// <returns>
    /// A collection of commands that move the robot.
    /// </returns>
    /// <response code="200">Returns Ok with the list of robot commands that move</response>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpGet("move")] 
    public IEnumerable<RobotCommand> GetMoveCommandsOnly()
    {
        // Filter commands to include only move commands
        return RobotCommandDataAccess.GetMoveCommandsOnly();
    }

    /// <summary>
    /// Retrieves a robot command by its ID.
    /// </summary>
    /// <param name="id">The ID of the robot command to retrieve.</param>
    /// <returns> The robot command object if robot command is found.
    /// </returns>
    /// <response code="404">If the RobotCommand was not found</response>
    /// <response code="200">Returns Ok with the command object</response>>
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpGet("{id}", Name = "GetRobotCommand")] 
    public IActionResult GetRobotCommandById(int id)
    {
        // Find the command with the specified id
        var command = RobotCommandDataAccess.GetRobotCommandById(id);

        // If command is not found, return NotFound
        if (command == null)
        {
            return NotFound();
        }

        // If command is found, return Ok with the command object
        return Ok(command);
    }

    /// <summary>
    /// Creates a robot command.
    /// </summary>
    /// <param name="newCommand">A new robot command from the HTTP request.</param>
    /// <returns>A newly created robot command</returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /api/robot-commands
    ///     {
    ///        "Name": "DOWN",
    ///        "IsMoveCommand": false
    ///     }
    ///
    /// </remarks>
    /// <response code="201">Returns the newly created robot command</response>
    /// <response code="400">If the robot command is null, or addition fails</response>
    /// <response code="409">If a robot command with the same name already exists.</response>
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [HttpPost] 
    public IActionResult AddRobotCommand(RobotCommand newCommand)
    {
        if (newCommand == null) //A new command value is needed for this method (so check for null value)
        {
            return BadRequest();
        }

        // Check if the command name already exists, if so return with no edits to database
        RobotCommand? robotExists = RobotCommandDataAccess.GetRobotCommandByName(newCommand.Name);
        if (robotExists != null && robotExists.Name == newCommand.Name)
        {
            return Conflict();
        }

        try
        {
            //Try to add the robot command to the database
            RobotCommandDataAccess.AddRobotCommand(newCommand);
            RobotCommand? addedNewCommand = RobotCommandDataAccess.GetRobotCommandByName(newCommand.Name);
            // Return a GET endpoint resource URI (the URI of the added new command)
            return CreatedAtRoute("GetRobotCommand", new { id = addedNewCommand.Id }, addedNewCommand);
        }
        catch
        {
            // Return BadRequest if addition fails
            return BadRequest();
        }

    }

    /// <summary>
    /// Updates an existing robot command.
    /// </summary>
    /// <param name="id">The ID of the command to update.</param>
    /// <param name="updatedCommand">The updated command details from the HTTP request.</param>
    /// <returns>NoContent if successful update for Robot Command</returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     PUT /api/robot-commands/:id
    ///     {
    ///        "Name": "DIAGONAL",
    ///        "Description": "Description of the new command",
    ///        "IsMoveCommand": false
    ///     }
    ///
    /// </remarks>
    /// <response code="404">If the robot command isn't found in database</response>
    /// <response code="204">No Content if Robot command is updated sucessfully</response>
    /// <response code="400">If update failed.</response>
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPut("{id}")] 
    public IActionResult UpdateRobotCommand(int id, RobotCommand updatedCommand)
    {
        // Find the command by id
        var existingCommand = RobotCommandDataAccess.GetRobotCommandById(id);

        // If command with specified id does not exist, return NotFound
        if (existingCommand == null)
        {
            return NotFound();
        }


        // Try to update the existing command with details from updatedCommand
        try
        {
            RobotCommandDataAccess.UpdateRobotCommand(id, updatedCommand);

            // Return NoContent if successful update
            return NoContent();
        }
        catch
        {
            // Return BadRequest if update fails
            return BadRequest();
        }


    }

    /// <summary>
    /// Deletes an existing robot command.
    /// </summary>
    /// <param name="id">The ID of the command to delete.</param>
    /// <returns>NoContent if successful deletion.</returns>
    /// <response code="404">If the robot command isn't found in database</response>
    /// <response code="204">No Content if Robot command was deleted</response>
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [Authorize(Policy = "AdminOnly")]
    [HttpDelete("{id}")] 
    public IActionResult DeleteRobotCommand(int id)
    {
        // Find the command by id
        var commandToRemove = RobotCommandDataAccess.GetRobotCommandById(id);

        // If command with this id doesn't exist, return NotFound
        if (commandToRemove == null)
        {
            return NotFound();
        }

        //Delete the robot command at this id
        RobotCommandDataAccess.DeleteRobotCommand(id);
        return NoContent();
    }

}
