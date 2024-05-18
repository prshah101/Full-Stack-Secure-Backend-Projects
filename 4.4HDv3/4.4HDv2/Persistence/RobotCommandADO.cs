using Npgsql;
using robot_controller_api;
namespace robot_controller_api.Persistence;
public class RobotCommandADO: IRobotCommandDataAccess
{
    // Connection string for connecting to the database
    private const string CONNECTION_STRING =
   "Host=localhost;Username=postgres;Password=password;Database=sit331";

    // A method to retrieve all the robot commands
    public List<RobotCommand> GetRobotCommands()
    {
        // Create a list to store the retrieved robot commands
        var robotCommands = new List<RobotCommand>();
        // Establish a connection to the database
        using var conn = new NpgsqlConnection(CONNECTION_STRING);
        conn.Open();
        // Create a SQL command to select all robot commands
        using var cmd = new NpgsqlCommand("SELECT * FROM robot_command", conn);
        // Execute the command and retrieve data by reading it
        using var dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            // Retrieve values from the data reader. Right hand side are the values store in the Server, LHS are how they will be refered to in this class.
            int id = (int)dr["id"];
            string name = (string)dr["Name"];
            string? description;
            if (dr.IsDBNull(dr.GetOrdinal("description"))) //If description is null in the database, store it as null
            {
                description = null;
            }
            else
            {
                description = (string)dr["description"]; //else store the actual value
            }
            bool isMoveCommand = (bool)dr["is_move_command"];
            DateTime createdDate = (DateTime)dr["created_date"];
            DateTime modifiedDate = (DateTime)dr["modified_date"];

            // Create a new RobotCommand object with the commands read from the server
            RobotCommand robotCommand = new RobotCommand(id, name, isMoveCommand, createdDate, modifiedDate, description);

            //Add the Robot Command to the list
            robotCommands.Add(robotCommand);
        }
        return robotCommands;

    }

    //A method to return only Move commands
    public List<RobotCommand> GetMoveCommandsOnly()
    {
        // Create a list to store the retrieved robot commands
        var robotCommands = new List<RobotCommand>();
        // Establish a connection to the database
        using var conn = new NpgsqlConnection(CONNECTION_STRING);
        conn.Open();
        // Create a SQL command to select all robot commands where ismovecommand is true
        using var cmd = new NpgsqlCommand("SELECT * FROM robot_command WHERE is_move_command= @IsMoveCommand", conn);
        cmd.Parameters.AddWithValue("@IsMoveCommand", true);
        // Execute the command and retrieve data by reading it
        using var dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            int id = (int)dr["id"];
            string name = (string)dr["Name"];
            string? description;
            if (dr.IsDBNull(dr.GetOrdinal("description")))
            {
                description = null;
            }
            else
            {
                description = (string)dr["description"];
            }
            bool isMoveCommand = (bool)dr["is_move_command"];
            DateTime createdDate = (DateTime)dr["created_date"];
            DateTime modifiedDate = (DateTime)dr["modified_date"];

            // Create a new RobotCommand object with the commands read from the server
            RobotCommand robotCommand = new RobotCommand(id, name, isMoveCommand, createdDate, modifiedDate, description);

            //Add the Robot Command to the list
            robotCommands.Add(robotCommand);
        }
        return robotCommands;
    }

    // A method to retrieve a robot commands by id
    public RobotCommand? GetRobotCommandById(int id)
    {
        using var conn = new NpgsqlConnection(CONNECTION_STRING);
        conn.Open();
        //Use the proper SQL command to get the robot command where the id is the specified value
        using var cmd = new NpgsqlCommand("SELECT * FROM robot_command WHERE id = @Id", conn);
        cmd.Parameters.AddWithValue("@Id", id);

        using var dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            string name = (string)dr["Name"];
            string? description;
            if (dr.IsDBNull(dr.GetOrdinal("description")))
            {
                description = null;
            }
            else
            {
                description = (string)dr["description"];
            }
            bool isMoveCommand = (bool)dr["is_move_command"];
            DateTime createdDate = (DateTime)dr["created_date"];
            DateTime modifiedDate = (DateTime)dr["modified_date"];

            // Create a new RobotCommand object with the commands read from the server
            RobotCommand robotCommand = new RobotCommand(id, name, isMoveCommand, createdDate, modifiedDate, description);

            return robotCommand;
        }
        return null;
    }

    // A method to retrieve a robot commands by Name
    public RobotCommand? GetRobotCommandByName(string Name)
    {
        using var conn = new NpgsqlConnection(CONNECTION_STRING);
        conn.Open();
        using var cmd = new NpgsqlCommand("SELECT * FROM robot_command WHERE \"Name\" LIKE @NAME", conn);
        cmd.Parameters.AddWithValue("@Name", Name);

        using var dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            int id = (int)dr["id"];
            string name = (string)dr["Name"];
            string? description;
            if (dr.IsDBNull(dr.GetOrdinal("description")))
            {
                description = null;
            }
            else
            {
                description = (string)dr["description"];
            }
            bool isMoveCommand = (bool)dr["is_move_command"];
            DateTime createdDate = (DateTime)dr["created_date"];
            DateTime modifiedDate = (DateTime)dr["modified_date"];

            // Create a new RobotCommand object with the commands read from the server
            RobotCommand robotCommand = new RobotCommand(id, name, isMoveCommand, createdDate, modifiedDate, description);

            return robotCommand;
        }
        return null;
    }


    //Update the Robot Command
    public void UpdateRobotCommand(int id, RobotCommand updatedCommand)
    {
        using var conn = new NpgsqlConnection(CONNECTION_STRING);
        conn.Open();

        // Create a SQL command to update a robot command with specific values, at a specified id
        using var cmd = new NpgsqlCommand("UPDATE robot_command SET  \"Name\" = @Name, is_move_command = @IsMoveCommand, \"description\" = @Description, modified_date = @ModifiedDate WHERE id = @Id", conn);
        cmd.Parameters.AddWithValue("@Id", id);
        cmd.Parameters.AddWithValue("@Name", updatedCommand.Name);
        cmd.Parameters.AddWithValue("@IsMoveCommand", updatedCommand.IsMoveCommand);
        cmd.Parameters.AddWithValue("@Description", updatedCommand.Description);
        cmd.Parameters.AddWithValue("@ModifiedDate", DateTime.Now);

        cmd.ExecuteNonQuery();
    }

    //Method to insert a new Robot Command into the database
    public void AddRobotCommand(RobotCommand updatedCommand)
    {
        int newId;

        using var conn = new NpgsqlConnection(CONNECTION_STRING);
        conn.Open();

        // Create a SQL command to insert a robot command with specific values
        using var cmd = new NpgsqlCommand("INSERT INTO robotcommand (\"Name\", is_move_command, created_date, modified_date) VALUES (@Name, @IsMoveCommand, @CreatedDate, @ModifiedDate)", conn);
        cmd.Parameters.AddWithValue("@Name", updatedCommand.Name);
        cmd.Parameters.AddWithValue("@IsMoveCommand", updatedCommand.IsMoveCommand);
        cmd.Parameters.AddWithValue("@ModifiedDate", DateTime.Now);
        cmd.Parameters.AddWithValue("@CreatedDate", DateTime.Now);

        cmd.ExecuteNonQuery();
    }

    //Method to delete a Robot Command from database
    public void DeleteRobotCommand(int id)
    {
        using var conn = new NpgsqlConnection(CONNECTION_STRING);
        conn.Open();

        // Create a SQL command to delete a robot command at a specific id
        using var cmd = new NpgsqlCommand("DELETE FROM robot_command WHERE id = @Id", conn);
        cmd.Parameters.AddWithValue("@Id", id);

        cmd.ExecuteNonQuery();
    }




}
