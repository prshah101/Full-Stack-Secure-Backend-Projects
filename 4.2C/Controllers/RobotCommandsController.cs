using Microsoft.AspNetCore.Mvc;
using robot_controller_api.Persistence;

namespace robot_controller_api.Controllers;

[ApiController]//5
[Route("api/robot-commands")]
public class RobotCommandsController : ControllerBase
{
    private readonly IRobotCommandDataAccess _robotCommandsRepo;
    public RobotCommandsController(IRobotCommandDataAccess
    robotCommandsRepo)
    {
        _robotCommandsRepo = robotCommandsRepo;
    }

    [HttpGet("")]
    public IEnumerable<RobotCommand> GetAllRobotCommands()
    {
        //Return all the robot commands using the method GetRobotCommands() from the class _robotCommandsRepo
        return _robotCommandsRepo.GetRobotCommands();
    }

    [HttpGet("move")] //8 //// return a filtered _commands field here after you filter out non-move commands.

    public IEnumerable<RobotCommand> GetMoveCommandsOnly()
    {
        // Filter commands to include only move commands
        return _robotCommandsRepo.GetMoveCommandsOnly();
    }

    [HttpGet("{id}", Name = "GetRobotCommand")] //9 //Based on ID, return a command
    public IActionResult GetRobotCommandById(int id)
    {
        // Find the command with the specified id
        var command = _robotCommandsRepo.GetRobotCommandById(id);

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

        // Check if the command name already exists, if so return with no edits to database
        RobotCommand? robotExists = _robotCommandsRepo.GetRobotCommandByName(newCommand.Name);
        if (robotExists != null && robotExists.Name == newCommand.Name)
        {
            return Conflict();
        }

        try
        {
            //Try to add the robot command to the database
            _robotCommandsRepo.AddRobotCommand(newCommand);
            RobotCommand? addedNewCommand = _robotCommandsRepo.GetRobotCommandByName(newCommand.Name);
            // Return a GET endpoint resource URI (the URI of the added new command)
            return CreatedAtRoute("GetRobotCommand", new { id = addedNewCommand.Id }, addedNewCommand);
        }
        catch
        {
            // Return BadRequest if addition fails
            return BadRequest();
        }

    }


    [HttpPut("{id}")] //11 //This endpoint modifys an existing command
    public IActionResult UpdateRobotCommand(int id, RobotCommand updatedCommand)
    {
        // Find the command by id
        var existingCommand = _robotCommandsRepo.GetRobotCommandById(id);

        // If command with specified id does not exist, return NotFound
        if (existingCommand == null)
        {
            return NotFound();
        }


        // Try to update the existing command with details from updatedCommand
        try
        {
            _robotCommandsRepo.UpdateRobotCommand(id, updatedCommand);

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
        var commandToRemove = _robotCommandsRepo.GetRobotCommandById(id);

        // If command with this id doesn't exist, return NotFound
        if (commandToRemove == null)
        {
            return NotFound();
        }

        //Delete the robot command at this id
        _robotCommandsRepo.DeleteRobotCommand(id);
        return NoContent();
    }

}
