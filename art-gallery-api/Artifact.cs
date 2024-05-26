using System;

public class Artifact
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Artist { get; set; }
    public string? Category { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }

    // Constructor
    public Artifact(int id, string name, string? description, string? artist, string? category, DateTime createdDate, DateTime modifiedDate)
    {
        Id = id;
        Name = name;
        Description = description;
        Artist = artist;
        Category = category;
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

    public void SetDescription(string description)
    {
        Description = description;
    }

    public string? GetDescription()
    {
        return Description;
    }

    public void SetArtist(string artist)
    {
        Artist = artist;
    }

    public string? GetArtist()
    {
        return Artist;
    }

    public void SetCategory(string category)
    {
        Category = category;
    }

    public string? GetCategory()
    {
        return Category;
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
