using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using robot_controller_api.Persistence;

public class BasicAuthenticationHandler :
AuthenticationHandler<AuthenticationSchemeOptions>
{
    public
   BasicAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions>
   options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) :
   base(options, logger, encoder, clock)
    {
    }
    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        base.Response.Headers.Add("WWW-Authenticate", @"Basic realm=""Access to the robot controller.""");
        var authHeader = base.Request.Headers["Authorization"].ToString();


        if (!Request.Headers.ContainsKey("Authorization"))
        {
            Response.StatusCode = 401;
            return Task.FromResult(AuthenticateResult.Fail("Unauthorized"));
        }

        if (string.IsNullOrEmpty(authHeader))
        {
            Response.StatusCode = 401;
            return Task.FromResult(AuthenticateResult.Fail("Unauthorized"));
        }

        if (!authHeader.StartsWith("Basic ", StringComparison.OrdinalIgnoreCase))
        {
            Response.StatusCode = 401;
            return Task.FromResult(AuthenticateResult.Fail("Unauthorized"));
        }

        // Authentication logic will be here. (Step 1)
        var token = authHeader.Substring(6);
        // Convert the Base64 encoded string to a byte array
        byte[] bytes = Convert.FromBase64String(token);
        // Convert the byte array to a regular UTF-8 string (Step 2)
        string credentials = Encoding.UTF8.GetString(bytes);
        // Split the credentials string by colon (:) (Step 3)

        var credentialsTest = credentials.Split(':');

        if (credentialsTest.Length != 2){
            return Task.FromResult(AuthenticateResult.Fail("Unauthorized"));
        }

        string[] parts = credentialsTest;
        string email = parts[0];
        string password = parts[1];

        //Get the user from the database with an email from the retrieved credentials. (Step 4)
        UserModel user = UserModelDataAccess.GetUserByEmail(email);

        // If the user with email doesn't exist, set this (Step 5)
        if (user == null)
        {
            Response.StatusCode = 401;
            return Task.FromResult(AuthenticateResult.Fail($"Authentication failed."));
        }

        //Initialize a new PasswordHasher object to verify the user's password (Step 6)
        var hasher = new PasswordHasher<UserModel>();
        var pwVerificationResult = hasher.VerifyHashedPassword(user,
        user.PasswordHash, password);

        // If the password is verified, build an array of claims about the user that 
        // can be used for further authorization (Step 7)
        var claims = new[]
        {
                new Claim("name", $"{user.FirstName} {user.LastName}"),
                new Claim(ClaimTypes.Role, user.Role)
                // any other claims that you think might be useful
            };
        //Construct and ClaimsIdentity for a specific authentication type that we use in this task, create a
        //ClaimPrincipal and issue an AuthencticationTicket for that principal and return a successful
        //authentication result. (Step 8)
        var identity = new ClaimsIdentity(claims, "Basic");
        var claimsPrincipal = new ClaimsPrincipal(identity);
        var authTicket = new AuthenticationTicket(claimsPrincipal, Scheme.Name);
        return Task.FromResult(AuthenticateResult.Success(authTicket));
    }
}