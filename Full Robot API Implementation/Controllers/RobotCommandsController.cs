using Microsoft.AspNetCore.Mvc;

namespace robot_controller_api.Controllers;

[ApiController]//5
[Route("api/robot-commands")]
public class RobotCommandsController : ControllerBase
{
    private static readonly List<RobotCommand> _commands = new List<RobotCommand>
    {
        // Adding RobotCommand for every command that exists in the legacy system (3)
        new RobotCommand(1, "LEFT", true, DateTime.Now, DateTime.Now),
        new RobotCommand(2, "RIGHT", true, DateTime.Now, DateTime.Now),
        new RobotCommand(3, "MOVE", true, DateTime.Now, DateTime.Now),
        new RobotCommand(4, "PLACE", false, DateTime.Now, DateTime.Now),
        new RobotCommand(5, "REPORT", false, DateTime.Now, DateTime.Now)
    };

    [HttpGet]//4//return all commands as JSON 
    public IEnumerable<RobotCommand> GetAllRobotCommands()
    {
        return _commands;
    }

    [HttpGet("move")] //8 //// return a filtered _commands field here after you filter out non-move commands.

    public IEnumerable<RobotCommand> GetMoveCommandsOnly()
    {
        // Filter commands to include only move commands
        return _commands.Where(command => command.IsMoveCommand);
    }

    [HttpGet("{id}", Name = "GetRobotCommand")] //9 //Based on ID, return a command
    public IActionResult GetRobotCommandById(int id)
    {
        // Find the command with the specified id
        var command = _commands.FirstOrDefault(c => c.Id == id);

        // If command is not found, return NotFound
        if (command == null)
        {
            return NotFound();
        }

        // If command is found, return Ok with the command object
        return Ok(command);
    }

    [HttpPost] //10 //Add a command to the array
    public IActionResult AddRobotCommand(RobotCommand newCommand)
    {
        if (newCommand == null) //A new command value is needed for this method (so check for null value)
        {
            return BadRequest();
        }

        // Check if the command name already exists, if so return with no edits
        if (_commands.Any(c => c.Name == newCommand.Name))
        {
            return Conflict();
        }

        // Assign a new unique Id (don't blindly use the one from newCommand)
        int newId = _commands.Count + 1;
        newCommand.Id = newId;

        // Set CreatedDate and ModifiedDate to DateTime.Now
        newCommand.CreatedDate = DateTime.Now;
        newCommand.ModifiedDate = DateTime.Now;

        // Add the new command to the list
        _commands.Add(newCommand);

        // Return a GET endpoint resource URI (the URI of the added new command)
        return CreatedAtRoute("GetRobotCommand", new { id = newCommand.Id }, newCommand);
    }


    [HttpPut("{id}")] //11 //This endpoint modifys an existing command
    public IActionResult UpdateRobotCommand(int id, RobotCommand updatedCommand)
    {
        // Find the command by id
        var existingCommand = _commands.FirstOrDefault(c => c.Id == id);

        // If command with specified id does not exist, return NotFound
        if (existingCommand == null)
        {
            return NotFound();
        }

        // Try to update the existing command with details from updatedCommand
        try
        {
            existingCommand.Name = updatedCommand.Name;
            existingCommand.Description = updatedCommand.Description;
            existingCommand.IsMoveCommand = updatedCommand.IsMoveCommand;

            // Set ModifiedDate to DateTime.Now
            existingCommand.ModifiedDate = DateTime.Now;

            // Return NoContent if successful update
            return NoContent();
        }
        catch
        {
            // Return BadRequest if update fails
            return BadRequest();
        }


    }

    [HttpDelete("{id}")] //12  //Delete an existing command
    public IActionResult DeleteRobotCommand(int id)
    {
        // Find the command by id
        var commandToRemove = _commands.FirstOrDefault(c => c.Id == id);

        // If command with this id doesn't exist, return NotFound
        if (commandToRemove == null)
        {
            return NotFound();
        }

        // Remove the command from _commands
        _commands.Remove(commandToRemove);

        // Return NoContent if deletion was successful
        return NoContent();
    }

}
