using Npgsql;
using System;
using robot_controller_api;
namespace robot_controller_api.Persistence
{
    public static class UserModelDataAccess
    {
        private const string CONNECTION_STRING = "Host=localhost;Username=postgres;Password=password;Database=sit331";

        public static List<UserModel> GetUsers()
        {
            var users = new List<UserModel>();
            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();
            using var cmd = new NpgsqlCommand("SELECT * FROM public.user", conn);
            using var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                int id = (int)dr["id"];
                string email = (string)dr["email"];
                string firstName = (string)dr["firstname"];
                string lastName = (string)dr["lastname"];
                string passwordHash = (string)dr["passwordhash"];
                string? description = dr.IsDBNull(dr.GetOrdinal("description")) ? null : (string)dr["description"];
                string? role = dr.IsDBNull(dr.GetOrdinal("role")) ? null : (string)dr["role"];
                DateTime createdDate = (DateTime)dr["createddate"];
                DateTime modifiedDate = (DateTime)dr["modifieddate"];

                UserModel userModel = new UserModel(id, email, firstName, lastName, passwordHash, description, role, createdDate, modifiedDate);

                users.Add(userModel);
            }
            return users;
        }

        public static List<UserModel> GetAdminUsersOnly()
        {
            var adminUsers = new List<UserModel>();
            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();
            using var cmd = new NpgsqlCommand("SELECT * FROM public.user WHERE role = 'Admin'", conn);
            using var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                int id = (int)dr["id"];
                string email = (string)dr["email"];
                string firstName = (string)dr["firstname"];
                string lastName = (string)dr["lastname"];
                string passwordHash = (string)dr["passwordhash"];
                string? description = dr.IsDBNull(dr.GetOrdinal("description")) ? null : (string)dr["description"];
                string? role = dr.IsDBNull(dr.GetOrdinal("role")) ? null : (string)dr["role"];
                DateTime createdDate = (DateTime)dr["createddate"];
                DateTime modifiedDate = (DateTime)dr["modifieddate"];

                UserModel user = new UserModel(id, email, firstName, lastName, passwordHash, description, role, createdDate, modifiedDate);

                adminUsers.Add(user);
            }
            return adminUsers;
        }


        public static UserModel GetUserById(int id)
        {
            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();
            using var cmd = new NpgsqlCommand("SELECT * FROM public.user WHERE id = @Id", conn);
            cmd.Parameters.AddWithValue("@Id", id);
            using var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                string email = (string)dr["email"];
                string firstName = (string)dr["firstname"];
                string lastName = (string)dr["lastname"];
                string passwordHash = (string)dr["passwordhash"];
                string description = dr["description"] != DBNull.Value ? (string)dr["description"] : null;
                string role = dr["role"] != DBNull.Value ? (string)dr["role"] : null;
                DateTime createdDate = (DateTime)dr["createddate"];
                DateTime modifiedDate = (DateTime)dr["modifieddate"];

                // Create a new User object with the commands read from the server
                UserModel userModel = new UserModel(id, email, firstName, lastName, passwordHash, description, role, createdDate, modifiedDate);

                return userModel;
            }
            return null;
        }

        public static UserModel GetUserByEmail(string email)
        {
            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();
            using var cmd = new NpgsqlCommand("SELECT * FROM public.user WHERE email = @Email", conn);
            cmd.Parameters.AddWithValue("@Email", email);
            using var dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                int id = (int)dr["id"];
                string firstName = (string)dr["firstname"];
                string lastName = (string)dr["lastname"];
                string passwordHash = (string)dr["passwordhash"];
                string description = dr["description"] != DBNull.Value ? (string)dr["description"] : null;
                string role = dr["role"] != DBNull.Value ? (string)dr["role"] : null;
                DateTime createdDate = (DateTime)dr["createddate"];
                DateTime modifiedDate = (DateTime)dr["modifieddate"];

                UserModel userModel = new UserModel(id, email, firstName, lastName, passwordHash, description, role, createdDate, modifiedDate);
                return userModel;
            }
            return null;
        }


        public static void AddUser(UserModel user)
        {
            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();
            using var cmd = new NpgsqlCommand("INSERT INTO public.user (email, firstname, lastname, passwordhash, description, role, createddate, modifieddate) VALUES (@Email, @FirstName, @LastName, @PasswordHash, @Description, @Role, @CreatedDate, @ModifiedDate)", conn);
            cmd.Parameters.AddWithValue("@Email", user.Email);
            cmd.Parameters.AddWithValue("@FirstName", user.FirstName);
            cmd.Parameters.AddWithValue("@LastName", user.LastName);
            cmd.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);
            cmd.Parameters.AddWithValue("@Description", (object)user.Description ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Role", (object)user.Role ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
            cmd.Parameters.AddWithValue("@ModifiedDate", DateTime.Now);
            cmd.ExecuteNonQuery();
        }

        public static void UpdateUser(int id, UserModel updatedUser)
        {
            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();
            using var cmd = new NpgsqlCommand("UPDATE public.user SET firstname = @FirstName, lastname = @LastName, description = @Description, role = @Role, modifieddate = @ModifiedDate WHERE id = @Id", conn);
            cmd.Parameters.AddWithValue("@Id", id);
            cmd.Parameters.AddWithValue("@FirstName", updatedUser.FirstName);
            cmd.Parameters.AddWithValue("@LastName", updatedUser.LastName);
            cmd.Parameters.AddWithValue("@Description", (object)updatedUser.Description ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Role", (object)updatedUser.Role ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@ModifiedDate", DateTime.Now);
            cmd.ExecuteNonQuery();
        }

        public static void DeleteUser(int id)
        {
            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();
            using var cmd = new NpgsqlCommand("DELETE FROM public.user WHERE id = @Id", conn);
            cmd.Parameters.AddWithValue("@Id", id);
            cmd.ExecuteNonQuery();
        }

        public static void UpdateUserCredentials(int id, UserModel loginModel)
        {
            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();
            using var cmd = new NpgsqlCommand("UPDATE public.user SET email = @Email, passwordhash = @PasswordHash, modifieddate = @ModifiedDate WHERE id = @Id", conn);
            cmd.Parameters.AddWithValue("@Id", id);
            cmd.Parameters.AddWithValue("@Email", loginModel.Email);
            cmd.Parameters.AddWithValue("@PasswordHash", loginModel.PasswordHash);
            cmd.Parameters.AddWithValue("@ModifiedDate", DateTime.Now);
            cmd.ExecuteNonQuery();
        }
    }
}
