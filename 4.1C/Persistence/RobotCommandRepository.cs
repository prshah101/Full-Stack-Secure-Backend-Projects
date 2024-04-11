using Npgsql;
using robot_controller_api.Persistence;

namespace robot_controller_api.Persistence
{
    public class RobotCommandRepository : IRobotCommandDataAccess, IRepository
    {

        private IRepository _repo => this;

        public List<RobotCommand> GetRobotCommands()
        {
            var commands = _repo.ExecuteReader<RobotCommand>("SELECT * FROM public.robotcommand");
            return commands;
        }

        public List<RobotCommand> GetMoveCommandsOnly()
        {
            return _repo.ExecuteReader<RobotCommand>("SELECT * FROM public.robotcommand WHERE ismovecommand = @IsMoveCommand", new NpgsqlParameter("@IsMoveCommand", true));
        }

        public RobotCommand? GetRobotCommandById(int id)
        {
            return _repo.ExecuteReader<RobotCommand>("SELECT * FROM robotcommand WHERE id = @Id", new NpgsqlParameter("@Id", id)).FirstOrDefault();
        }

        public RobotCommand? GetRobotCommandByName(string name)
        {
            return _repo.ExecuteReader<RobotCommand>("SELECT * FROM robotcommand WHERE Name LIKE @Name", new NpgsqlParameter("@Name", name)).FirstOrDefault();
        }

        public void UpdateRobotCommand(int id, RobotCommand updatedCommand)
        {
            var sqlParams = new NpgsqlParameter[]
            {
                new NpgsqlParameter("id", id),
                new NpgsqlParameter("name", updatedCommand.Name),
                new NpgsqlParameter("description", updatedCommand.Description ?? (object)DBNull.Value),
                new NpgsqlParameter("ismovecommand", updatedCommand.IsMoveCommand)
            };

            _repo.ExecuteReader<RobotCommand>(
                @"UPDATE robotcommand 
                  SET name=@name, 
                      description=@description, 
                      ismovecommand=@ismovecommand, 
                      modifieddate=current_timestamp 
                  WHERE id=@id;",
                sqlParams
            );
        }

        public void AddRobotCommand(RobotCommand updatedCommand)
        {
            var sqlParams = new NpgsqlParameter[]
            {
                new NpgsqlParameter("name", updatedCommand.Name),
                new NpgsqlParameter("ismovecommand", updatedCommand.IsMoveCommand),
                new NpgsqlParameter("createddate", DateTime.Now),
                new NpgsqlParameter("modifieddate", DateTime.Now)
            };

            _repo.ExecuteReader<RobotCommand>(
                "INSERT INTO robotcommand (\"Name\", ismovecommand, createddate, modifieddate) VALUES (@name, @ismovecommand, @createddate, @modifieddate);",
                sqlParams
            );
        }

        public void DeleteRobotCommand(int id)
        {
            var sqlParams = new NpgsqlParameter[]
            {
                new NpgsqlParameter("id", id)
            };

            _repo.ExecuteReader<RobotCommand>(
                "DELETE FROM robotcommand WHERE id = @id;",
                sqlParams
            );
        }
    }
}
