using Npgsql;
using robot_controller_api.Persistence;

namespace robot_controller_api.Persistence
{
     // This class implements data access operations for robot commands using ADO.NET  
    public class RobotCommandRepository : IRobotCommandDataAccess, IRepository
    {

        private IRepository _repo => this;

        // Method to retrieve all robot commands from the database
        public List<RobotCommand> GetRobotCommands()
        {
            var commands = _repo.ExecuteReader<RobotCommand>("SELECT * FROM robot_command");
            return commands;
        }

        // Method to retrieve only move commands from the database
        public List<RobotCommand> GetMoveCommandsOnly()
        {
            var sqlParams = new NpgsqlParameter[]
            {
                new NpgsqlParameter("@IsMoveCommand", true)
            };

            var result = _repo.ExecuteReader<RobotCommand>(
                "SELECT * FROM robot_command WHERE is_move_command = @IsMoveCommand",
                sqlParams
            );

            return result;
        }

        // Method to retrieve a robot command from the database, based on its ID 
        public RobotCommand? GetRobotCommandById(int id)
        {
            var sqlParams = new NpgsqlParameter[]
            {
                 new NpgsqlParameter("@Id", id)
            };

            return _repo.ExecuteReader<RobotCommand>(
                "SELECT * FROM robot_command WHERE id = @Id",
                sqlParams
            ).FirstOrDefault();
        }

        // Method to retrieve a robot command from the database, based on its name 
        public RobotCommand? GetRobotCommandByName(string name)
        {
            var sqlParams = new NpgsqlParameter[]
            {
             new NpgsqlParameter("@Name", name)
            };

            return _repo.ExecuteReader<RobotCommand>(
                "SELECT * FROM robot_command WHERE Name LIKE @Name",
                sqlParams
            ).FirstOrDefault();
        }

        // Method to update an existing robot command in the database, based on id
        public void UpdateRobotCommand(int id, RobotCommand updatedCommand)
        {
            var sqlParams = new NpgsqlParameter[]{
            new("id", id),
            new("name", updatedCommand.Name),
            new("description", updatedCommand.Description ?? (object)DBNull.Value),
            new("ismovecommand", updatedCommand.IsMoveCommand)
            };

            var result = _repo.ExecuteReader<RobotCommand>(
                "UPDATE robot_command SET Name=@name, description=@description, is_move_command = @ismovecommand, modified_date = current_timestamp WHERE id = @id RETURNING *; ",
            sqlParams)
            .Single();
        }

        // Method to add a new robot command to the database
        public void AddRobotCommand(RobotCommand updatedCommand)
        {
            var sqlParams = new NpgsqlParameter[]
            {
                new NpgsqlParameter("Name", updatedCommand.Name),
                new NpgsqlParameter("Ismovecommand", updatedCommand.IsMoveCommand),
                new NpgsqlParameter("Description", updatedCommand.Description ?? (object)DBNull.Value),
                new NpgsqlParameter("Createddate", DateTime.Now),
                new NpgsqlParameter("Modifieddate", DateTime.Now)
            };

            _repo.ExecuteReader<RobotCommand>(
                "INSERT INTO robot_command (Name, is_move_command, description, created_date, modified_date) VALUES (@Name, @Ismovecommand, @Description, @Createddate, @Modifieddate);",
                sqlParams
            );
        }

        // Method to delete a robot command from the database by its ID
        public void DeleteRobotCommand(int id)
        {
            var sqlParams = new NpgsqlParameter[]
            {
                new NpgsqlParameter("id", id)
            };

            _repo.ExecuteReader<RobotCommand>(
                "DELETE FROM robot_command WHERE id = @id;",
                sqlParams
            );
        }
    }
}
