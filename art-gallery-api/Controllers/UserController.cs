using Microsoft.AspNetCore.Mvc;
using art_gallery_api.Persistence;
using System;
using System.Collections.Generic;

namespace art_gallery_api.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        [HttpGet("")]
        public IEnumerable<User> GetAllUsers()
        {
            return UserDataAccess.GetUsers();
        }

        [HttpGet("{id}", Name = "GetUser")]
        public IActionResult GetUserById(int id)
        {
            var user = UserDataAccess.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPost]
        public IActionResult AddUser(User newUser)
        {
            if (newUser == null)
            {
                return BadRequest();
            }

            // Check if the user already exists by email
            var existingUser = UserDataAccess.GetUsers().FirstOrDefault(u => u.Email == newUser.Email);
            if (existingUser != null)
            {
                return Conflict("User with the same email already exists.");
            }

            try
            {
                UserDataAccess.AddUser(newUser);
                //User? addedNewUser = UserDataAccess.GetUserByEmail(newUser.Email);
                return CreatedAtRoute("GetUser", new { id = newUser.Id }, newUser);
            }
            catch
            {
                return BadRequest();
            }
        }


        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, User updatedUser)
        {
            var existingUser = UserDataAccess.GetUserById(id);
            if (existingUser == null)
            {
                return NotFound();
            }

            try
            {
                UserDataAccess.UpdateUser(id, updatedUser);
                return NoContent();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            var userToRemove = UserDataAccess.GetUserById(id);
            if (userToRemove == null)
            {
                return NotFound();
            }

            UserDataAccess.DeleteUser(id);
            return NoContent();
        }
    }
}
