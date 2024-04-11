namespace robot_controller_api.Persistence
{
    public interface IRobotCommandDataAccess
    {
        List<RobotCommand> GetRobotCommands();
        List<RobotCommand> GetMoveCommandsOnly();
        RobotCommand? GetRobotCommandById(int id);
        RobotCommand? GetRobotCommandByName(string name);
        void AddRobotCommand(RobotCommand newCommand);
        void UpdateRobotCommand(int id, RobotCommand updatedCommand);
        void DeleteRobotCommand(int id);
    }
}
