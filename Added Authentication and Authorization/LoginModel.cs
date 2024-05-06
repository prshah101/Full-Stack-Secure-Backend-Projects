public class LoginModel
{
    public string Email { get; }
    public string Password { get; }

    public LoginModel(string email, string password)
    {
        Email = email;
        Password = password;
    }

    // Override Equals and GetHashCode to compare objects by their values

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        var other = (LoginModel)obj;
        return Email == other.Email && Password == other.Password;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Email, Password);
    }
}
