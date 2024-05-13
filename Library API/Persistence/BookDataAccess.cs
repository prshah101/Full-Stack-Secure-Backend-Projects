using Npgsql;
using System;
using System.Collections.Generic;

namespace LibraryControllerApi.Persistence
{
    public static class BookDataAccess
    {
        // Connection string for connecting to the database
        private const string CONNECTION_STRING = "Host=localhost;Username=postgres;Password=password;Database=library";

        // Gets all Books from the database
        public static List<Book> GetAllBooks()
        {
            var Books = new List<Book>();
            // Establish a connection to the database
            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();
            // Execute an SQL command to select all Books
            using var cmd = new NpgsqlCommand("SELECT * FROM Book", conn);
            // Executes the command and retrieves data from database
            using var dr = cmd.ExecuteReader();
            // Reads each row of the result
            while (dr.Read())
            {
                // Retrieve values from the data reader. Right hand side are the values store in the Server, LHS are how they will be refered to in this class.
                int id = (int)dr["id"];
                string title = (string)dr["title"];
                string author = dr["author"] as string;
                DateTime publicationYear = (DateTime)dr["createddate"];

                // Create a Book object and add it to the list
                Book Book = new Book(id, title, author, publicationYear);
                Books.Add(Book);
            }
            return Books;
        }

        // Retrieve a Book by its ID from the database
        public static Book GetBookById(int id)
        {
            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();
            // Execute SQL command to select the Book at a specific id
            using var cmd = new NpgsqlCommand("SELECT * FROM Books WHERE id = @Id", conn);
            cmd.Parameters.AddWithValue("@Id", id);
            using var dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                string title = (string)dr["title"];
                string author = dr["author"] as string;
                DateTime publicationYear = (DateTime)dr["createddate"];

                // Create a Book object and add it to the list
                Book Book = new Book(id, title, author, publicationYear);
            }
            return null;
        }

        // Retrieve a Book by its name from the database
        public static Book GetBookByTitle(string Name)
        {
            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();
            using var cmd = new NpgsqlCommand("SELECT * FROM Books WHERE \"name\" LIKE @NAME", conn);
            cmd.Parameters.AddWithValue("@name", Name);
            using var dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                // Retrieve values from the data reader. Right hand side are the values store in the Server, LHS are how they will be refered to in this class.
                int id = (int)dr["id"];
                string title = (string)dr["title"];
                string author = dr["author"] as string;
                DateTime publicationYear = (DateTime)dr["createddate"];

                // Create a Book object and add it to the list
                Book Book = new Book(id, title, author, publicationYear);
            }
            return null;
        }


        // Update an existing Book in the database
        public static void UpdateBook(int id, Book updatedBook)
        {
            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();
            using var cmd = new NpgsqlCommand("UPDATE Books SET title = @title WHERE id = @Id", conn);
            cmd.Parameters.AddWithValue("@Id", id);
            cmd.Parameters.AddWithValue("@title", updatedBook.Title);
            cmd.ExecuteNonQuery();
        }

        // Add a new Book to the database
        public static void AddBook(Book newBook)
        {
            int newId;
            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();

            // Retrieve the highest ID currently to create the next ID in the database
            using (var getMaxIdCmd = new NpgsqlCommand("SELECT MAX(id) FROM Book", conn))
            {
                object maxIdObj = getMaxIdCmd.ExecuteScalar();
                // If maxIdObj == DBNull.Value is true, maxId  = 0. Else, maxId  = maxIdObj as an integer
                int maxId = maxIdObj == DBNull.Value ? 0 : Convert.ToInt32(maxIdObj);
                newId = maxId + 1;
            }

            using var cmd = new NpgsqlCommand("INSERT INTO Books (name, title, author, publicationyear) VALUES (@title, @Author, @PublicationYear)", conn);
            cmd.Parameters.AddWithValue("@title", newBook.Title);
            cmd.Parameters.AddWithValue("@Author", newBook.Author);
            cmd.Parameters.AddWithValue("@PublicationYear", DateTime.Now);
            cmd.ExecuteNonQuery();
        }

        // Delete a Book from the database by its ID
        public static void DeleteBook(int id)
        {
            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();
            using var cmd = new NpgsqlCommand("DELETE FROM Books WHERE id = @Id", conn);
            cmd.Parameters.AddWithValue("@Id", id);
            cmd.ExecuteNonQuery();
        }
    }
}
