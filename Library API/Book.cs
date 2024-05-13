using System;

namespace LibraryControllerApi
{
    public class Book
    {
        // Property for storing the ID of the book
        public int Id { get; set; }
        
        // Title of the book
        public string Title { get; set; }
        
        // Author of the book
        public string Author { get; set; }
        
        // Publication year of the book
        public DateTime PublicationYear { get; set; }
        
        // Constructor to initialize the Book
        public Book(int id, string title, string author, DateTime publicationYear)
        {
            Id = id;
            Title = title;
            Author = author;
            PublicationYear = publicationYear;
        }
    }
}
