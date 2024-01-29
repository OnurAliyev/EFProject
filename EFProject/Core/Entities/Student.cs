namespace EFProject.Core.Entities;

public class Student
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Surname { get; set; } = null!;
    public int Age { get; set; }
    public DateTime CreatedDate { get; set; }
    public ICollection<StudentGroup> StudentGroups { get; set; }
}
