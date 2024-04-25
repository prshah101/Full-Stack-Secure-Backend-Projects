using Npgsql;
using robot_controller_api.Persistence;
using robot_controller_api.Models;
using Microsoft.EntityFrameworkCore;

namespace robot_controller_api.Persistence
{
     // This class implements data access operations for robot commands using ADO.NET  
    public class RobotCommandEF : RobotContext, IRobotCommandDataAccess
    {
        public RobotCommandEF(DbContextOptions<RobotContext> options):base(options){ 

        }
        private RobotContext _robotContext = new();


        // Method to retrieve all robot commands from the database
        public List<RobotCommand> GetRobotCommands()
        {
            return _robotContext.RobotCommands.OrderBy(x=>x.Id).ToList();
        }

        // Method to retrieve only move commands from the database
        public List<RobotCommand> GetMoveCommandsOnly()
        {
            var commands = _robotContext.RobotCommands
                .Where(x=>x.IsMoveCommand == true)
                .ToList();
            return commands;
        }

        // Method to retrieve a robot command from the database, based on its ID 
        public RobotCommand? GetRobotCommandById(int id)
        {
            return (RobotCommand?)_robotContext.RobotCommands.Where(x=>x.Id == id);
        }

        // Method to retrieve a robot command from the database, based on its name 
        public RobotCommand? GetRobotCommandByName(string name)
        {
            return (RobotCommand?)_robotContext.RobotCommands.Where(x=>x.Name == name);
        }

        // Method to update an existing robot command in the database, based on id
        public void UpdateRobotCommand(int id, RobotCommand updatedCommand)
        {
            // Find the existing robot command by its ID
            var existingCommand = _robotContext.RobotCommands.FirstOrDefault(c => c.Id == id);
            
            // If the command with the given ID is found
            if (existingCommand != null)
            {
                // Update the properties of the existing command
                existingCommand.Name = updatedCommand.Name;
                existingCommand.Description = updatedCommand.Description;
                existingCommand.IsMoveCommand = updatedCommand.IsMoveCommand;
                existingCommand.ModifiedDate = DateTime.Now; 
                
                // Save changes to the database
                _robotContext.SaveChanges();
            }
        }

        // Method to add a new robot command to the database
        public void AddRobotCommand(RobotCommand newCommand)
        {
            // // Create a new RobotCommand object with the provided data
            // var newCommand = new RobotCommand
            // {
            //     Name = updatedCommand.Name,
            //     Description = updatedCommand.Description,
            //     IsMoveCommand = updatedCommand.IsMoveCommand,
            //     CreatedDate = DateTime.Now, 
            //     ModifiedDate = DateTime.Now 
            // };

            // // Add the new command to the DbSet in the context
            // _robotContext.RobotCommands.Add(newCommand);

            // // Save changes to the database
            // _robotContext.SaveChanges();

            _robotContext.RobotCommands.Add(newCommand);
            _robotContext.SaveChanges();
        }

        // Method to delete a robot command from the database by its ID
        public void DeleteRobotCommand(int id)
        {
            // Find the existing robot command by its ID
            var commandToDelete = _robotContext.RobotCommands.FirstOrDefault(c => c.Id == id);

            // If the command with the given ID is found
            if (commandToDelete != null)
            {
                // Remove the command from the DbSet in the context
                _robotContext.RobotCommands.Remove(commandToDelete);

                // Save changes to the database
                _robotContext.SaveChanges();
            }
        }
    }
}
