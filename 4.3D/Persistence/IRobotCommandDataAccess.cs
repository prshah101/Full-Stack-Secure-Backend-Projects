using robot_controller_api.Models;

namespace robot_controller_api.Persistence
{
    // The interface to define data access operations for robot command entities
    public interface IRobotCommandDataAccess
    {
       // Retrieve all robot commands from the data store, so return as a List of RobotCommands
        List<RobotCommand> GetRobotCommands();

        // Retrieve only move commands from the data store, so return as a List of RobotCommands
        List<RobotCommand> GetMoveCommandsOnly();

        // Retrieve a robot command by its unique identifier, so return a RobotCommand object
        RobotCommand? GetRobotCommandById(int id);

        // Retrieve a robot command by its name, so return a RobotCommand object
        RobotCommand? GetRobotCommandByName(string name);

        // Add a new robot command to the data store, return nothing
        void AddRobotCommand(RobotCommand newCommand);

        // Update an existing robot command in the data store, return nothing
        void UpdateRobotCommand(int id, RobotCommand updatedCommand);

        // Delete a robot command from the data store by its identifier, return nothing
        void DeleteRobotCommand(int id);

    }
}
