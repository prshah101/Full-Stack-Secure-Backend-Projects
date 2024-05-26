using System;
public class User
{
    public int Id { get; set; }
    public string? Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? PasswordHash { get; set; }
    public string? Description { get; set; }
    public string? Role { get; set; }
    public string? Membership { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }

    // Constructor
    public User(int id, string email, string firstName, string lastName, string passwordHash, string? description, string? role, string? membership, DateTime createdDate, DateTime modifiedDate)
    {
        Id = id;
        Email = email;
        FirstName = firstName;
        LastName = lastName;
        PasswordHash = passwordHash;
        Description = description;
        Role = role;
        Membership = membership;
        CreatedDate = createdDate;
        ModifiedDate = modifiedDate;
    }

    // Getter and setter methods
    public void SetId(int id)
    {
        Id = id;
    }

    public int GetId()
    {
        return Id;
    }

    public void SetEmail(string email)
    {
        Email = email;
    }

    public string GetEmail()
    {
        return Email;
    }

    public void SetFirstName(string firstName)
    {
        FirstName = firstName;
    }

    public string GetFirstName()
    {
        return FirstName;
    }

    public void SetLastName(string lastName)
    {
        LastName = lastName;
    }

    public string GetLastName()
    {
        return LastName;
    }

    public void SetPasswordHash(string passwordHash)
    {
        PasswordHash = passwordHash;
    }

    public string GetPasswordHash()
    {
        return PasswordHash;
    }

    public void SetDescription(string description)
    {
        Description = description;
    }

    public string GetDescription()
    {
        return Description;
    }

    public void SetRole(string role)
    {
        Role = role;
    }

    public string GetRole()
    {
        return Role;
    }

    public void SetMembership(string membership)
    {
        Membership = membership;
    }

    public string GetMembership()
    {
        return Membership;
    }

    public void SetCreatedDate(DateTime createdDate)
    {
        CreatedDate = createdDate;
    }

    public DateTime GetCreatedDate()
    {
        return CreatedDate;
    }

    public void SetModifiedDate(DateTime modifiedDate)
    {
        ModifiedDate = modifiedDate;
    }

    public DateTime GetModifiedDate()
    {
        return ModifiedDate;
    }
}
