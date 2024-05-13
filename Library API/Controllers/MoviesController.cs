using Microsoft.AspNetCore.Mvc;
using LibraryControllerApi.Persistence;

namespace LibraryControllerApi.Controllers;

[ApiController]//5
[Route("api/library-movies")]
public class MoviesController : ControllerBase
{
    [HttpGet("")]
    public IEnumerable<Movie> GetAllMovies()
    {
        //Return all the robot movies using the method GetMovies() from the class MovieDataAccess
        return MovieDataAccess.GetMovies();
    }


    [HttpGet("{id}", Name = "GetMovie")] //9 //Based on ID, return a movie
    public IActionResult GetMovieById(int id)
    {
        // Find the movie with the specified id
        var movie = MovieDataAccess.GetMovieById(id);

        // If movie is not found, return NotFound
        if (movie == null)
        {
            return NotFound();
        }

        // If movie is found, return Ok with the movie object
        return Ok(movie);
    }

    [HttpPost] //10 //Add a movie to the array
    public IActionResult AddMovie(Movie newMovie)
    {
        if (newMovie == null) //A new movie value is needed for this method (so check for null value)
        {
            return BadRequest();
        }

        // Check if the movie name already exists, if so return with no edits to database
        Movie? movieExists = MovieDataAccess.GetMovieByName(newMovie.Title);
        if (movieExists != null && movieExists.Title == newMovie.Title)
        {
            return Conflict();
        }

        try
        {
            //Try to add the robot movie to the database
            MovieDataAccess.AddMovie(newMovie);
            return Ok();
        }
        catch
        {
            // Return BadRequest if addition fails
            return BadRequest();
        }

    }


    [HttpPut("{id}")] //11 //This endpoint modifys an existing movie
    public IActionResult UpdateMovie(int id, Movie updatedMovie)
    {
        // Find the movie by id
        var existingmovie = MovieDataAccess.GetMovieById(id);

        // If movie with specified id does not exist, return NotFound
        if (existingmovie == null)
        {
            return NotFound();
        }


        // Try to update the existing movie with details from updatedMovie
        try
        {
            MovieDataAccess.UpdateMovie(id, updatedMovie);

            // Return NoContent if successful update
            return NoContent();
        }
        catch
        {
            // Return BadRequest if update fails
            return BadRequest();
        }


    }

    [HttpDelete("{id}")] //12  //Delete an existing movie
    public IActionResult DeleteMovie(int id)
    {
        // Find the movie by id
        var movieToRemove = MovieDataAccess.GetMovieById(id);

        // If movie with this id doesn't exist, return NotFound
        if (movieToRemove == null)
        {
            return NotFound();
        }

        //Delete the robot movie at this id
        MovieDataAccess.DeleteMovie(id);
        return NoContent();
    }

}
