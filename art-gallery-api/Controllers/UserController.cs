using Microsoft.AspNetCore.Mvc;

namespace robot_controller_api.Controllers;

[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    private static readonly List<User> _commands = new List<User>
    {
        // Adding RobotCommand for every command that exists in the legacy system (3)
        new User(1, "example@example.com", "John", "Doe", "password","Default description", "User", "Basic", DateTime.Now, DateTime.Now),
    };


    [HttpGet]//4//return all commands as JSON 
    public IEnumerable<User> GetAllRobotCommands()
    {
        return _commands;
    }
}
