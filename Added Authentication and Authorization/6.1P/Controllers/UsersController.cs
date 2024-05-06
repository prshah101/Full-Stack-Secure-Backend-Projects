using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using robot_controller_api.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

[Route("api/users")]
[ApiController]
public class UserController : ControllerBase
{
    /// <summary>
    /// Retrieves all users.
    /// </summary>
    /// <returns>
    /// A collection of all users.
    /// </returns>
    /// <response code="200">Returns Ok with the list of users</response>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [AllowAnonymous]
    [HttpGet]
    public IEnumerable<UserModel> GetAllUsers()
    {
        return UserModelDataAccess.GetUsers();
    }

    /// <summary>
    /// Retrieves only users that move are admins.
    /// </summary>
    /// <returns>
    /// A collection of users that are admins.
    /// </returns>
    /// <response code="200">Returns Ok with the list of users that are admins</response>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpGet("admin")]
    public IEnumerable<UserModel> GetAdminUsers()
    {
        return UserModelDataAccess.GetAdminUsersOnly();
    }

    
    /// <summary>
    /// Retrieves a User by its ID.
    /// </summary>
    /// <param name="id">The ID of the User to retrieve.</param>
    /// <returns> The User object if User is found.
    /// </returns>
    /// <response code="404">If the User was not found</response>
    /// <response code="200">Returns Ok with the User object</response>>
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpGet("{id}", Name = "GetUserById")]
    public ActionResult<UserModel> GetUserById(int id)
    {
        // Find the User with the specified id
        var UserModel = UserModelDataAccess.GetUserById(id);

        // If User is not found, return NotFound
        if (UserModel == null)
        {
            return NotFound();
        }
        return Ok(UserModel);
    }

    /// <summary>
    /// Creates a user.
    /// </summary>
    /// <param name="newUserModel">A new user from the HTTP request.</param>
    /// <returns>A newly created user</returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /api/users
    ///     {
    ///        "email": "example@example.com",
    ///        "firstName": "Mary"
    ///        "lastName": "Ann"
    ///        "passwordHash": "password123",
    ///        "description": "Some description",
    ///        "role": "Admin"
    ///     }
    ///
    /// </remarks>
    /// <response code="201">Returns the newly created User</response>
    /// <response code="400">If the User is null, or addition fails</response>
    /// <response code="409">If a User with the same email already exists.</response>
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [HttpPost]
    public IActionResult AddUser(UserModel newUserModel)
    {
        if (newUserModel == null) //A new user value is needed for this method (so check for null value)
        {
            return BadRequest();
        }

        // Check if the user already exists, if so return with no edits to database
        UserModel? userExists = UserModelDataAccess.GetUserByEmail(newUserModel.Email);
        if (userExists != null && userExists.Email == newUserModel.Email)
        {
            return Conflict();
        }

        try
        {
            //Hash the password
            var hasher = new PasswordHasher<UserModel>();
            var pwHash = hasher.HashPassword(newUserModel, newUserModel.PasswordHash);
            var pwVerificationResult = hasher.VerifyHashedPassword(newUserModel, pwHash,
            newUserModel.PasswordHash);
            newUserModel.PasswordHash = pwHash;
            //Try to add the user to the database
            UserModelDataAccess.AddUser(newUserModel);
            UserModel? addedNewUser = UserModelDataAccess.GetUserByEmail(newUserModel.Email);
            // Return a GET endpoint resource URI (the URI of the added new user)
            return CreatedAtRoute("GetUserById", new { id = addedNewUser.Id }, addedNewUser);
        }
        catch
        {
            // Return BadRequest if addition fails
            return BadRequest();
        }
    }

    /// <summary>
    /// Updates an existing User.
    /// </summary>
    /// <param name="id">The ID of the User to update.</param>
    /// <param name="updatedUser">The updated User details from the HTTP request.</param>
    /// <returns>NoContent if successful update for User</returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     PUT /api/users/:id
    ///     {
    ///        "firstName": "John",
    ///        "lastName": "Sharma",
    ///        "passwordHash": "password123",
    ///        "description": "Some description2",
    ///        "role": "User"
    ///     }
    ///
    /// </remarks>
    /// <response code="404">If the User isn't found in database</response>
    /// <response code="204">No Content if User is updated sucessfully</response>
    /// <response code="400">If update failed.</response>
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPut("{id}")]
    public IActionResult UpdateUser(int id, UserModel updatedUser)
    {
        // Find the user by id
        var existingUser = UserModelDataAccess.GetUserById(id);

        // If user with specified id does not exist, return NotFound
        if (existingUser == null)
        {
            return NotFound();
        }


        // Try to update the existing user with details from updatedUser
        try
        {
            UserModelDataAccess.UpdateUser(id, updatedUser);

            // Return NoContent if successful update
            return NoContent();
        }
        catch
        {
            // Return BadRequest if update fails
            return BadRequest();
        }
    }

    /// <summary>
    /// Deletes an existing User.
    /// </summary>
    /// <param name="id">The ID of the User to delete.</param>
    /// <returns>NoContent if successful deletion.</returns>
    /// <response code="404">If the User isn't found in database</response>
    /// <response code="204">No Content if User was deleted</response>
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [Authorize(Policy = "AdminOnly")]
    [HttpDelete("{id}")]
    public IActionResult DeleteUser(int id)
    {
        // Find the User by id
        var userToRemove = UserModelDataAccess.GetUserById(id);

        // If User with this id doesn't exist, return NotFound
        if (userToRemove == null)
        {
            return NotFound();
        }

        //Delete the User at this id
        UserModelDataAccess.DeleteUser(id);
        return NoContent();
    }

    /// <summary>
    /// Updates an existing User's credentials.
    /// </summary>
    /// <param name="id">The ID of the User to update.</param>
    /// <param name="loginModel">The updated User login details from the HTTP request.</param>
    /// <returns>A newly updated User</returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     PATCH /api/users/:id
    ///     {
    ///        "email": "example@example2.com",
    ///        "password": "Password"
    ///     }
    ///
    /// </remarks>
    /// <response code="404">If the User isn't found in database</response>
    /// <response code="400">If update failed.</response>
    /// <response code="201">Returns the newly updated User</response>
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [HttpPatch("{id}")]
    public IActionResult UpdateUserCredentials(int id, LoginModel loginModel)
    {
        if (loginModel == null) //A new login value is needed for this method (so check for null value)
        {
            return BadRequest();
        }

        // Find the user by id
        var userExists = UserModelDataAccess.GetUserById(id);

        // If user with specified id does not exist, return NotFound
        if (userExists == null)
        {
            return NotFound();
        }

        try
        {
            userExists.PasswordHash = loginModel.Password;
            //Hash the password
            var hasher = new PasswordHasher<UserModel>();
            var pwHash = hasher.HashPassword(userExists, userExists.PasswordHash);
            var pwVerificationResult = hasher.VerifyHashedPassword(userExists, pwHash,
            userExists.PasswordHash);
            userExists.PasswordHash = pwHash;

            //Try to update the user in the database
            UserModelDataAccess.UpdateUserCredentials(userExists.Id, userExists);
            UserModel? addedNewUser = UserModelDataAccess.GetUserByEmail(userExists.Email);
            // Return a GET endpoint resource URI (the URI of the user with updated credentials)
            return CreatedAtRoute("GetUserById", new { id = addedNewUser.Id }, addedNewUser);
        }
        catch
        {
            // Return BadRequest if update fails
            return BadRequest();
        }
    }
}
