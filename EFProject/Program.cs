using EFProject.Core.Entities;
using EFProject.DataAccess;
using EFProject.Helpers;
using Microsoft.EntityFrameworkCore;

string appStart = "Application started...";
string Welcome = "Welcome!";
Console.SetCursorPosition((Console.WindowWidth - appStart.Length) / 2, Console.CursorTop);
Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine(appStart);
Console.SetCursorPosition((Console.WindowWidth - Welcome.Length) / 2, Console.CursorTop);
Console.ForegroundColor = ConsoleColor.Blue;
Console.WriteLine(Welcome);
Console.ResetColor();
AppDbContext context = new();
List<Student> students = new();
List<Group> groups = new();
List<StudentGroup> studentGroups = new();

bool runApp = true;
while (runApp)
{
    Console.ForegroundColor = ConsoleColor.DarkYellow;
    Console.WriteLine("1 == Create student\n" +
                      "2 == Create group\n" +
                      "3 == Add student to group\n" +
                      "4 == Update student\n" +
                      "5 == Update group\n" +
                      "0 == Close App\n" +
                      " ");
    Console.ResetColor();
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.Write("Chosse the option >> ");
    Console.ResetColor();

    string? option = Console.ReadLine();
    int IntOption;
    bool IsInt = int.TryParse(option, out IntOption);
    if (IsInt)
    {
        if (IntOption >= 0 && IntOption <= 5)
        {
            switch (IntOption)
            {
                case (int)Menu.StCreate:
                    try
                    {
                        Console.Write("Enter student name: ");
                        string? studentName = Console.ReadLine();
                        Console.Write("Enter student surname: ");
                        string? studentSurname = Console.ReadLine();
                        int studentAge;
                        do
                        {
                            Console.Write("Enter student age (must be greater than 16): ");
                        } while (!int.TryParse(Console.ReadLine(), out studentAge) || studentAge <= 16);

                        Student student = new()
                        {
                            Name = studentName,
                            Surname = studentSurname,
                            Age = studentAge,
                            CreatedDate = DateTime.Now,
                        };
                        students.Add(student);
                        await context.Students.AddAsync(student);
                        await context.SaveChangesAsync();
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Student created successfully!");
                        Console.ResetColor();
                    }
                    catch (Exception ex)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(ex.Message);
                        Console.ResetColor();
                        goto case (int)Menu.StCreate;
                    }
                    break;
                case (int)Menu.GpCreate:
                    try
                    {
                        Console.WriteLine("Enter group name:");
                        string groupName=Console.ReadLine();
                        int groupCapacity;
                        do
                        {
                            Console.Write("Enter group capacity (must be greater than 10): ");
                        } while (!int.TryParse(Console.ReadLine(), out groupCapacity) || groupCapacity <= 10);
                        Group existingGroup=await context.Groups.FirstOrDefaultAsync(g => g.Name == groupName);
                        if(existingGroup is null)
                        {
                            Group group = new()
                            {
                                Name = groupName,
                                Capacity=groupCapacity,
                                CreatedTime = DateTime.Now

                            };
                            groups.Add(group);
                            await context.Groups.AddAsync(group);
                            await context.SaveChangesAsync();
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("Group created succesfully!");
                            Console.ResetColor();
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"Group with name '{groupName}' already exists.");
                            Console.ResetColor();
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(ex.Message);
                        Console.ResetColor();
                        goto case (int)Menu.GpCreate;
                    }
                    break;
            }
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\nPlease enter correct number!\n");
            Console.ResetColor();
        }
    }
    else
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("\nPlease enter correct format!\n");
        Console.ResetColor();
    }
}


