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
            var sqlParams = new NpgsqlParameter[]
            {
                new NpgsqlParameter("@IsMoveCommand", true)
            };

            var result = _repo.ExecuteReader<RobotCommand>(
                "SELECT * FROM public.robotcommand WHERE ismovecommand = @IsMoveCommand",
                sqlParams
            );

            return result;
        }


        public RobotCommand? GetRobotCommandById(int id)
        {
            var sqlParams = new NpgsqlParameter[]
            {
                 new NpgsqlParameter("@Id", id)
            };

            return _repo.ExecuteReader<RobotCommand>(
                "SELECT * FROM robotcommand WHERE id = @Id",
                sqlParams
            ).FirstOrDefault();
        }

        public RobotCommand? GetRobotCommandByName(string name)
        {
            var sqlParams = new NpgsqlParameter[]
            {
             new NpgsqlParameter("@Name", name)
            };

            return _repo.ExecuteReader<RobotCommand>(
                "SELECT * FROM robotcommand WHERE Name LIKE @Name",
                sqlParams
            ).FirstOrDefault();
        }


        public void UpdateRobotCommand(int id, RobotCommand updatedCommand)
        {
            var sqlParams = new NpgsqlParameter[]{
            new("id", id),
            new("name", updatedCommand.Name),
            new("description", updatedCommand.Description ?? (object)DBNull.Value),
            new("ismovecommand", updatedCommand.IsMoveCommand)
            };

            var result = _repo.ExecuteReader<RobotCommand>(
                "UPDATE robotcommand SET name=@name, description=@description, ismovecommand = @ismovecommand, modifieddate = current_timestamp WHERE id = @id RETURNING *; ",
            sqlParams)
            .Single();
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
