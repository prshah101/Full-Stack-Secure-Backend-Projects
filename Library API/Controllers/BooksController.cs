using Microsoft.AspNetCore.Mvc;
using LibraryControllerApi.Persistence;

using LibraryControllerApi.Controllers;
using LibraryControllerApi;

[ApiController]
[Route("api/library-books")]
public class BooksController : ControllerBase
{
    [HttpGet] // Return all Books as JSON
    public IEnumerable<Book> GetAllBooks()
    {
        //Return all the Books using the method GetAllBooks() from the class BookDataAccess
        return BookDataAccess.GetAllBooks();
    }

    [HttpGet("{id}", Name = "GetBook")] // Based on ID, return a Book
    public IActionResult GetBookById(int id)
    {
        // Retrieve the Book with the specified id
        Book Book = BookDataAccess.GetBookById(id);

        // If Book is not found, return NotFound
        if (Book == null)
        {
            return NotFound();
        }

        // If Book is found, return Ok with the Book object
        return Ok(Book);
    }

    [HttpPost] // Add a Book to the array
    public IActionResult AddBook(Book newBook)
    {
        if (newBook == null) //A new Book value is needed for this method (so check for null value)
        {
            return BadRequest();
        }

        // Check if the Book Title already exists, if so return with no edits
        Book? BookExists = BookDataAccess.GetBookByTitle(newBook.Title);
        if (BookExists != null && BookExists.Title == newBook.Title)
        {
            return Conflict();
        }

        try
        {
            //Try to add the Book command to the database
            BookDataAccess.AddBook(newBook);
            Book? addedNewBook = BookDataAccess.GetBookByTitle(newBook.Title);
            // Return a GET endpoint resource URI (the URI of the added new Book)
            return CreatedAtRoute("GetRobotCommand", new { id = addedNewBook.Id }, addedNewBook);
        }
        catch
        {
            // Return BadRequest if addition fails
            return BadRequest();
        }
    }

    [HttpPut("{id}")] // This endpoint modifies an existing Book
    public IActionResult UpdateBook(int id, Book updatedBook)
    {
        // Find the Book by id
        Book existingBook = BookDataAccess.GetBookById(id);

        // If Book with specified id does not exist, return NotFound
        if (existingBook == null)
        {
            return NotFound();
        }

        // Try to update the existing Book with details from updatedBook
        try
        {
            BookDataAccess.UpdateBook(id, updatedBook);

            return NoContent();
        }
        catch
        {
            // Return BadRequest if update fails
            return BadRequest();
        }
    }

    [HttpDelete("{id}")] // Delete an existing Book
    public IActionResult DeleteBook(int id)
    {
        // Find the Book by id
        Book Book = BookDataAccess.GetBookById(id);

        // If Book with this id doesn't exist, return NotFound
        if (Book == null)
        {
            return NotFound();
        }

        // Remove the Book from Books
        BookDataAccess.DeleteBook(id);

        // Return NoContent if deletion was successful
        return NoContent();
    }

}
