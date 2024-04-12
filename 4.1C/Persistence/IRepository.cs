using Npgsql;
namespace robot_controller_api.Persistence
{
    // This interface defines repository operations for interacting with the database
    public interface IRepository
    {
        private const string CONNECTION_STRING =
        "Host=localhost;Username=postgres;Password=password;Database=sit331";
        
        // Executes a SQL command and returns a list of entities
        public List<T> ExecuteReader<T>(string sqlCommand, NpgsqlParameter[] dbParams = null) where T : class, new()
        {
            var entities = new List<T>();

            // Establish a connection to the database
            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();
            using var cmd = new NpgsqlCommand(sqlCommand, conn);
            // Some of our SQL commands might have SQL parameters we will need to pass to DB. MS
            if (dbParams is not null)
            {
                // CommandType is unnecessary for PostgreSQL but can be used in other DB engines like Oracle or SQL Server. MS
                // cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddRange(dbParams.Where(x => x.Value is not null).ToArray());
            }

            // Executing the command and retrieving data
            using var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                var entity = new T();
                dr.MapTo(entity);
                entities.Add(entity);
            }

            // Return the list of entities
            return entities;
        }
    }
}
