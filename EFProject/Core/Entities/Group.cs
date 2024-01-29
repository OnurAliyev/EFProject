﻿namespace EFProject.Core.Entities;

public class Group
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int Capacity { get; set; }
    public ICollection<StudentGroup> StudentGroups { get; set; }
}
