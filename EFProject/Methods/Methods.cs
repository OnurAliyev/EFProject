using EFProject.Core.Entities;
using EFProject.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace EFProject.Methods;

public class Methods
{
    AppDbContext context = new();
    List<Student> students = new();
    List<Group> groups = new();
    List<StudentGroup> studentGroups = new();

    public async Task ShowAllStudents()
    {
        try
        {
            List<Student> allStudents = await context.Students.ToListAsync();
            if (allStudents.Count > 0)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\nList of all students:\n");
                Console.ResetColor();

                foreach (var student in allStudents)
                {
                    Console.WriteLine($"ID: {student.Id}, Name: {student.Name}, Surname: {student.Surname}, Age: {student.Age}");
                }

                Console.WriteLine();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\nNo students found.\n");
                Console.ResetColor();
            }
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(ex.Message);
            Console.ResetColor();
        }
    }
    public async Task ShowAllGroups()
    {
        try
        {
            List<Group> allGroups = await context.Groups.ToListAsync();
            if (allGroups.Count > 0)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\nList of all groups:\n");
                Console.ResetColor();

                foreach (var group in allGroups)
                {
                    Console.WriteLine($"ID: {group.Id}, Name: {group.Name}, Capacity: {group.Capacity}");
                }

                Console.WriteLine();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\nNo groups found.\n");
                Console.ResetColor();
            }
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(ex.Message);
            Console.ResetColor();
        }
    }

}