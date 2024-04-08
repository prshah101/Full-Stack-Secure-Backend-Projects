using Npgsql;
using robot_controller_api;
namespace robot_controller_api.Persistence;
public static class RobotCommandDataAccess
{
    // Connection string for connecting to the database
    private const string CONNECTION_STRING =
   "Host=localhost;Username=postgres;Password=password;Database=sit331";

    // A method to retrieve all the robot commands
    public static List<RobotCommand> GetRobotCommands()
    {
        // Create a list to store the retrieved robot commands
        var robotCommands = new List<RobotCommand>();
        // Establish a connection to the database
        using var conn = new NpgsqlConnection(CONNECTION_STRING);
        conn.Open();
        // Create a SQL command to select all robot commands
        using var cmd = new NpgsqlCommand("SELECT * FROM robotcommand", conn);
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
            bool isMoveCommand = (bool)dr["ismovecommand"];
            DateTime createdDate = (DateTime)dr["createddate"];
            DateTime modifiedDate = (DateTime)dr["modifieddate"];

            // Create a new RobotCommand object with the commands read from the server
            RobotCommand robotCommand = new RobotCommand(id, name, isMoveCommand, createdDate, modifiedDate, description);

            //Add the Robot Command to the list
            robotCommands.Add(robotCommand);
        }
        return robotCommands;

    }

    //A method to return only Move commands
    public static List<RobotCommand> GetMoveCommandsOnly()
    {
        // Create a list to store the retrieved robot commands
        var robotCommands = new List<RobotCommand>();
        // Establish a connection to the database
        using var conn = new NpgsqlConnection(CONNECTION_STRING);
        conn.Open();
        // Create a SQL command to select all robot commands where ismovecommand is true
        using var cmd = new NpgsqlCommand("SELECT * FROM robotcommand WHERE ismovecommand= @IsMoveCommand", conn);
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
            bool isMoveCommand = (bool)dr["ismovecommand"];
            DateTime createdDate = (DateTime)dr["createddate"];
            DateTime modifiedDate = (DateTime)dr["modifieddate"];

            // Create a new RobotCommand object with the commands read from the server
            RobotCommand robotCommand = new RobotCommand(id, name, isMoveCommand, createdDate, modifiedDate, description);

            //Add the Robot Command to the list
            robotCommands.Add(robotCommand);
        }
        return robotCommands;
    }

    // A method to retrieve a robot commands by id
    public static RobotCommand? GetRobotCommandById(int id)
    {
        using var conn = new NpgsqlConnection(CONNECTION_STRING);
        conn.Open();
        //Use the proper SQL command to get the robot command where the id is the specified value
        using var cmd = new NpgsqlCommand("SELECT * FROM robotcommand WHERE id = @Id", conn);
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
            bool isMoveCommand = (bool)dr["ismovecommand"];
            DateTime createdDate = (DateTime)dr["createddate"];
            DateTime modifiedDate = (DateTime)dr["modifieddate"];

            // Create a new RobotCommand object with the commands read from the server
            RobotCommand robotCommand = new RobotCommand(id, name, isMoveCommand, createdDate, modifiedDate, description);

            return robotCommand;
        }
        return null;
    }

    // A method to retrieve a robot commands by Name
    public static RobotCommand? GetRobotCommandByName(string Name)
    {
        using var conn = new NpgsqlConnection(CONNECTION_STRING);
        conn.Open();
        using var cmd = new NpgsqlCommand("SELECT * FROM robotcommand WHERE \"Name\" LIKE @NAME", conn);
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
            bool isMoveCommand = (bool)dr["ismovecommand"];
            DateTime createdDate = (DateTime)dr["createddate"];
            DateTime modifiedDate = (DateTime)dr["modifieddate"];

            // Create a new RobotCommand object with the commands read from the server
            RobotCommand robotCommand = new RobotCommand(id, name, isMoveCommand, createdDate, modifiedDate, description);

            return robotCommand;
        }
        return null;
    }


    //Update the Robot Command
    public static void UpdateRobotCommand(int id, RobotCommand updatedCommand)
    {
        using var conn = new NpgsqlConnection(CONNECTION_STRING);
        conn.Open();

        // Create a SQL command to update a robot command with specific values, at a specified id
        using var cmd = new NpgsqlCommand("UPDATE robotcommand SET  \"Name\" = @Name, ismovecommand = @IsMoveCommand, \"description\" = @Description, modifieddate = @ModifiedDate WHERE id = @Id", conn);
        cmd.Parameters.AddWithValue("@Id", id);
        cmd.Parameters.AddWithValue("@Name", updatedCommand.Name);
        cmd.Parameters.AddWithValue("@IsMoveCommand", updatedCommand.IsMoveCommand);
        cmd.Parameters.AddWithValue("@Description", updatedCommand.Description);
        cmd.Parameters.AddWithValue("@ModifiedDate", DateTime.Now);

        cmd.ExecuteNonQuery();
    }

    //Method to insert a new Robot Command into the database
    public static void AddRobotCommand(RobotCommand updatedCommand)
    {
        int newId;

        using var conn = new NpgsqlConnection(CONNECTION_STRING);
        conn.Open();

        // Retrieve the highest ID currently to create the next ID in the database
        using (var getMaxIdCmd = new NpgsqlCommand("SELECT MAX(id) FROM robotcommand", conn))
        {
            object maxIdObj = getMaxIdCmd.ExecuteScalar();
            // If maxIdObj == DBNull.Value is true, maxId  = 0. Else, maxId  = maxIdObj as an integer
            int maxId = maxIdObj == DBNull.Value ? 0 : Convert.ToInt32(maxIdObj);
            newId = maxId + 1;
        }

        // Create a SQL command to insert a robot command with specific values
        using var cmd = new NpgsqlCommand("INSERT INTO robotcommand (\"Name\", ismovecommand, createddate, modifieddate) VALUES (@Name, @IsMoveCommand, @CreatedDate, @ModifiedDate)", conn);
        cmd.Parameters.AddWithValue("@Id", newId);
        cmd.Parameters.AddWithValue("@Name", updatedCommand.Name);
        cmd.Parameters.AddWithValue("@IsMoveCommand", updatedCommand.IsMoveCommand);
        cmd.Parameters.AddWithValue("@ModifiedDate", DateTime.Now);
        cmd.Parameters.AddWithValue("@CreatedDate", DateTime.Now);

        cmd.ExecuteNonQuery();
    }

    //Method to delete a Robot Command from database
    public static void DeleteRobotCommand(int id)
    {
        using var conn = new NpgsqlConnection(CONNECTION_STRING);
        conn.Open();

        // Create a SQL command to delete a robot command at a specific id
        using var cmd = new NpgsqlCommand("DELETE FROM robotcommand WHERE id = @Id", conn);
        cmd.Parameters.AddWithValue("@Id", id);

        cmd.ExecuteNonQuery();
    }




}
