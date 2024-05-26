using System;
using System.Collections.Generic;
using System.Data.SQLite;
using art_gallery_api;

namespace art_gallery_api.Persistence
{
    public static class UserDataAccess
    {
        // Connection string for connecting to the SQLite database
        private const string CONNECTION_STRING = "Data Source=robot_controller.db;Version=3;";

        // A method to retrieve all the robot commands
        public static List<RobotCommand> GetRobotCommands()
        {
            var robotCommands = new List<RobotCommand>();

            using var conn = new SQLiteConnection(CONNECTION_STRING);
            conn.Open();

            using var cmd = new SQLiteCommand("SELECT * FROM robotcommand", conn);
            using var dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                var name = reader.GetString(0);
            }

            return robotCommands;
        }
    }
}