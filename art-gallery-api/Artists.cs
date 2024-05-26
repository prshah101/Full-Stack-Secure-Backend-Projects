using System;

public class Artist
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Genre { get; set; }
    public string? Biography { get; set; }
    public string? Country { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }

    // Constructor
    public Artist(int id, string name, string? genre, string? biography, string? country, DateTime createdDate, DateTime modifiedDate)
    {
        Id = id;
        Name = name;
        Genre = genre;
        Biography = biography;
        Country = country;
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

    public void SetName(string name)
    {
        Name = name;
    }

    public string GetName()
    {
        return Name;
    }

    public void SetGenre(string genre)
    {
        Genre = genre;
    }

    public string? GetGenre()
    {
        return Genre;
    }

    public void SetBiography(string biography)
    {
        Biography = biography;
    }

    public string? GetBiography()
    {
        return Biography;
    }

    public void SetCountry(string country)
    {
        Country = country;
    }

    public string? GetCountry()
    {
        return Country;
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
