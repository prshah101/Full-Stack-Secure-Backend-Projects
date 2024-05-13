using Npgsql;
using LibraryControllerApi;
namespace LibraryControllerApi.Persistence;
public static class MovieDataAccess
{
    // Connection string for connecting to the database
    private const string CONNECTION_STRING =
   "Host=localhost;Username=postgres;Password=password;Database=library";

    // A method to retrieve all the robot commands
    public static List<Movie> GetMovies()
    {
        // Create a list to store the retrieved robot commands
        var Movies = new List<Movie>();
        // Establish a connection to the database
        using var conn = new NpgsqlConnection(CONNECTION_STRING);
        conn.Open();
        // Create a SQL command to select all robot commands
        using var cmd = new NpgsqlCommand("SELECT * FROM Movies", conn);
        // Execute the command and retrieve data by reading it
        using var dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            // Retrieve values from the data reader. Right hand side are the values store in the Server, LHS are how they will be refered to in this class.
            int id = (int)dr["id"];
            string title = (string)dr["title"];
            string? description;
            if (dr.IsDBNull(dr.GetOrdinal("description"))) //If description is null in the database, store it as null
            {
                description = null;
            }
            else
            {
                description = (string)dr["description"]; //else store the actual value
            }
            DateTime createdDate = (DateTime)dr["createddate"];
            // Create a new Movie object with the commands read from the server
            Movie Movie = new Movie(id, title, createdDate, description);

            //Add the Robot Command to the list
            Movies.Add(Movie);
        }
        return Movies;

    }

    // A method to retrieve a robot commands by id
    public static Movie? GetMovieById(int id)
    {
        using var conn = new NpgsqlConnection(CONNECTION_STRING);
        conn.Open();
        //Use the proper SQL command to get the robot command where the id is the specified value
        using var cmd = new NpgsqlCommand("SELECT * FROM Movies WHERE id = @Id", conn);
        cmd.Parameters.AddWithValue("@Id", id);

        using var dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            string title = (string)dr["title"];
            string? description;
            if (dr.IsDBNull(dr.GetOrdinal("description"))) //If description is null in the database, store it as null
            {
                description = null;
            }
            else
            {
                description = (string)dr["description"]; //else store the actual value
            }
            DateTime createdDate = (DateTime)dr["createddate"];
            // Create a new Movie object with the commands read from the server
            Movie Movie = new Movie(id, title, createdDate, description);

            return Movie;
        }
        return null;
    }

    // A method to retrieve a robot commands by Name
    public static Movie? GetMovieByName(string title)
    {
        using var conn = new NpgsqlConnection(CONNECTION_STRING);
        conn.Open();
        using var cmd = new NpgsqlCommand("SELECT * FROM Movies WHERE \"title\" LIKE @TITLE", conn);
        cmd.Parameters.AddWithValue("@title", title);

        using var dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            // Retrieve values from the data reader. Right hand side are the values store in the Server, LHS are how they will be refered to in this class.
            int id = (int)dr["id"];
            title = (string)dr["title"];
            string? description;
            if (dr.IsDBNull(dr.GetOrdinal("description"))) //If description is null in the database, store it as null
            {
                description = null;
            }
            else
            {
                description = (string)dr["description"]; //else store the actual value
            }
            DateTime createdDate = (DateTime)dr["createddate"];
            // Create a new Movie object with the commands read from the server
            Movie Movie = new Movie(id, title, createdDate, description);

            return Movie;
        }
        return null;
    }


    //Update the Robot Command
    public static void UpdateMovie(int id, Movie updatedMovie)
    {
        using var conn = new NpgsqlConnection(CONNECTION_STRING);
        conn.Open();

        // Create a SQL command to update a robot command with specific values, at a specified id
        using var cmd = new NpgsqlCommand("UPDATE Movies SET title = @title WHERE id = @Id", conn);
        cmd.Parameters.AddWithValue("@Id", id);
        cmd.Parameters.AddWithValue("@title", updatedMovie.Title);

        cmd.ExecuteNonQuery();
    }

    //Method to insert a new Robot Command into the database
    public static void AddMovie(Movie updatedMovie)
    {
        int newId;

        using var conn = new NpgsqlConnection(CONNECTION_STRING);
        conn.Open();

        // Retrieve the highest ID currently to create the next ID in the database
        using (var getMaxIdCmd = new NpgsqlCommand("SELECT MAX(id) FROM Movies", conn))
        {
            object maxIdObj = getMaxIdCmd.ExecuteScalar();
            // If maxIdObj == DBNull.Value is true, maxId  = 0. Else, maxId  = maxIdObj as an integer
            int maxId = maxIdObj == DBNull.Value ? 0 : Convert.ToInt32(maxIdObj);
            newId = maxId + 1;
        }

        // Create a SQL command to insert a robot command with specific values
        using var cmd = new NpgsqlCommand("INSERT INTO Movies (id, title, createddate) VALUES (@Id, @title, @CreatedDate)", conn);
        cmd.Parameters.AddWithValue("@Id", newId);
        cmd.Parameters.AddWithValue("@title", updatedMovie.Title);
        cmd.Parameters.AddWithValue("@CreatedDate", DateTime.Now);

        cmd.ExecuteNonQuery();
    }

    //Method to delete a Robot Command from database
    public static void DeleteMovie(int id)
    {
        using var conn = new NpgsqlConnection(CONNECTION_STRING);
        conn.Open();

        // Create a SQL command to delete a robot command at a specific id
        using var cmd = new NpgsqlCommand("DELETE FROM Movies WHERE id = @Id", conn);
        cmd.Parameters.AddWithValue("@Id", id);

        cmd.ExecuteNonQuery();
    }




}
