namespace robot_controller_api;

public class RobotCommand
{
        // Property for storing the ID of the command
        //An Id as RobotCommand is an entity, not a value object in DDD terminology.
        public int Id { get; set; }

        //Name of the RobotCommand, e.g. LEFT, RIGHT, PLACE.
        public string Name { get; set; }

        // This property is nullable and can be null and not set.
        public string? Description { get; set; }

        // Flag indicating whether the robot command is a move command. E.g.
        //LEFT, RIGHT and MOVE are considered move commands. PLACE and REPORT are not.

        public bool IsMoveCommand { get; set; }

        //Date and time of RobotCommand 's creation
        public DateTime CreatedDate { get; set; }

        //Date and time when the RobotCommand was last modified.
        public DateTime ModifiedDate { get; set; }

        // Constructor to initialize the Robot Command
        public RobotCommand(
            int id, string name, bool isMoveCommand, DateTime createdDate,
            DateTime modifiedDate, string? description = null)
        {
                Id = id;
                Name = name;
                Description = description;
                IsMoveCommand = isMoveCommand;
                CreatedDate = createdDate;
                ModifiedDate = modifiedDate;
        }

        public RobotCommand()
        {
        }


}

