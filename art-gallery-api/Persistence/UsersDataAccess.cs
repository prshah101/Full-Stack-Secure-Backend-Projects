using Npgsql;
using System;
using System.Collections.Generic;

namespace art_gallery_api.Persistence
{
    public static class UserDataAccess
    {
        private const string CONNECTION_STRING = "Host=localhost;Username=postgres;Password=password;Database=diy";

        //Get a list of all Users
        public static List<User> GetUsers()
        {
            var users = new List<User>();
            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();
            using var cmd = new NpgsqlCommand("SELECT * FROM users", conn);
            using var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                int id = (int)dr["id"];
                string email = dr["email"].ToString();
                string firstName = dr["first_name"].ToString();
                string lastName = dr["last_name"].ToString();
                string? passwordHash = dr["password_hash"]?.ToString();
                string? description = dr["description"]?.ToString();
                string? role = dr["role"]?.ToString();
                string? membershipLevel = dr["membership_level"]?.ToString();
                DateTime createdDate = (DateTime)dr["created_date"];
                DateTime modifiedDate = (DateTime)dr["modified_date"];

                User user = new User(id, email, firstName, lastName, passwordHash, description, role, membershipLevel, createdDate, modifiedDate);
                users.Add(user);
            }
            return users;
        }

        public static User? GetUserById(int id) //Get a User by Id
        {
            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();
            using var cmd = new NpgsqlCommand("SELECT * FROM users WHERE id = @Id", conn);
            cmd.Parameters.AddWithValue("@Id", id);
            using var dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                string email = dr["email"].ToString();
                string firstName = dr["first_name"].ToString();
                string lastName = dr["last_name"].ToString();
                string? passwordHash = dr["password_hash"]?.ToString();
                string? description = dr["description"]?.ToString();
                string? role = dr["role"]?.ToString();
                string? membershipLevel = dr["membership_level"]?.ToString();
                DateTime createdDate = (DateTime)dr["created_date"];
                DateTime modifiedDate = (DateTime)dr["modified_date"];

                return new User(id, email, firstName, lastName, passwordHash, description, role, membershipLevel, createdDate, modifiedDate);
            }
            return null;
        }
        
        public static User? GetUserByEmail(string Email) //Get a User by Email
        {
            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();
            using var cmd = new NpgsqlCommand("SELECT * FROM users WHERE email = @Email", conn);
            cmd.Parameters.AddWithValue("@Email", Email);
            using var dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                int id = (int)dr["id"];
                string email = dr["email"].ToString();
                string firstName = dr["first_name"].ToString();
                string lastName = dr["last_name"].ToString();
                string? passwordHash = dr["password_hash"]?.ToString();
                string? description = dr["description"]?.ToString();
                string? role = dr["role"]?.ToString();
                string? membershipLevel = dr["membership_level"]?.ToString();
                DateTime createdDate = (DateTime)dr["created_date"];
                DateTime modifiedDate = (DateTime)dr["modified_date"];

                return new User(id, email, firstName, lastName, passwordHash, description, role, membershipLevel, createdDate, modifiedDate);
            }
            return null;
        }


        public static void AddUser(User user) //Add a User
        {
            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();
            using var cmd = new NpgsqlCommand("INSERT INTO users (email, first_name, last_name, password_hash, description, role, membership_level, created_date, modified_date) VALUES (@Email, @FirstName, @LastName, @PasswordHash, @Description, @Role, @MembershipLevel, @CreatedDate, @ModifiedDate)", conn);
            cmd.Parameters.AddWithValue("@Email", user.Email);
            cmd.Parameters.AddWithValue("@FirstName", user.FirstName);
            cmd.Parameters.AddWithValue("@LastName", user.LastName);
            cmd.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);
            cmd.Parameters.AddWithValue("@Description", user.Description ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Role", user.Role ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@MembershipLevel", user.MembershipLevel ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
            cmd.Parameters.AddWithValue("@ModifiedDate", DateTime.Now);

            cmd.ExecuteNonQuery();
        }

        //Update a User
        public static void UpdateUser(int id, User updatedUser)
        {
            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();
            using var cmd = new NpgsqlCommand("UPDATE users SET email = @Email, first_name = @FirstName, last_name = @LastName, password_hash = @PasswordHash, description = @Description, role = @Role, membership_level = @MembershipLevel, modified_date = @ModifiedDate WHERE id = @Id", conn);
            cmd.Parameters.AddWithValue("@Id", id);
            cmd.Parameters.AddWithValue("@Email", updatedUser.Email);
            cmd.Parameters.AddWithValue("@FirstName", updatedUser.FirstName);
            cmd.Parameters.AddWithValue("@LastName", updatedUser.LastName);
            cmd.Parameters.AddWithValue("@PasswordHash", updatedUser.PasswordHash);
            cmd.Parameters.AddWithValue("@Description", updatedUser.Description ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Role", updatedUser.Role ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@MembershipLevel", updatedUser.MembershipLevel ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@ModifiedDate", DateTime.Now);

            cmd.ExecuteNonQuery();
        }

        //Delete a User
        public static void DeleteUser(int id)
        {
            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();
            using var cmd = new NpgsqlCommand("DELETE FROM users WHERE id = @Id", conn);
            cmd.Parameters.AddWithValue("@Id", id);
            cmd.ExecuteNonQuery();
        }
    }
}
