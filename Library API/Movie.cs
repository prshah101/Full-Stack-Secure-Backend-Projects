namespace LibraryControllerApi
{
    public class Movie
    {
        // Property for storing the ID of the movie 
        public int Id { get; set; }
        
        // Title of the Movie
        public string Title { get; set; }
        
        // A brief description of the Movie
        public string? Description { get; set; }
        
        // Date and time of Movied's creation.
        public DateTime CreatedDate { get; set; }
        

        // Constructor to initialize the Movie 
        public Movie(
            int id, string title, DateTime createdDate, string? description = null)
            {
                Id = id;
                Title = title;
                Description = description;
                CreatedDate = createdDate;
        }
    }
}
