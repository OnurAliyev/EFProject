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
                            CreatedDate = DateTime.Now
                        };
                        students.Add(student);
                        await context.Students.AddAsync(student);
                        await context.SaveChangesAsync();
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("\nStudent created successfully!\n");
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
                        string groupName = Console.ReadLine();
                        int groupCapacity;
                        do
                        {
                            Console.Write("Enter group capacity (must be greater than 10): ");
                        } while (!int.TryParse(Console.ReadLine(), out groupCapacity) || groupCapacity <= 10);
                        Group existingGroup = await context.Groups.FirstOrDefaultAsync(g => g.Name == groupName);
                        if (existingGroup is null)
                        {
                            Group group = new()
                            {
                                Name = groupName,
                                Capacity = groupCapacity,
                                CreatedTime = DateTime.Now

                            };
                            groups.Add(group);
                            await context.Groups.AddAsync(group);
                            await context.SaveChangesAsync();
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("\nGroup created succesfully!\n");
                            Console.ResetColor();
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"\nGroup with name '{groupName}' already exists.\n");
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
                case (int)Menu.AddSt:
                    try
                    {
                        Console.Write("Enter student ID to add to the group: ");
                        if (int.TryParse(Console.ReadLine(), out int studentId))
                        {
                            // Verilən id ilə student var ya yox
                            Student existingStudent = await context.Students.FindAsync(studentId);
                            if (existingStudent is not null)
                            {
                                Console.Write("Enter group ID to add the student to: ");
                                if (int.TryParse(Console.ReadLine(), out int groupId))
                                {
                                    // Verilən id ilə group var ya yox
                                    Group existingGroup = await context.Groups.FindAsync(groupId);
                                    if (existingGroup is not null)
                                    {
                                        // Student artıq qrupdadırsa
                                        if (!await context.StudentGroups.AnyAsync(sg => sg.StudentId == studentId && sg.GroupId == groupId))
                                        {
                                            StudentGroup studentGroup = new()
                                            {
                                                StudentId = studentId,
                                                GroupId = groupId,
                                            };
                                            studentGroups.Add(studentGroup);
                                            await context.StudentGroups.AddAsync(studentGroup);
                                            await context.SaveChangesAsync();

                                            Console.ForegroundColor = ConsoleColor.Green;
                                            Console.WriteLine("\nStudent added to the group successfully!\n");
                                            Console.ResetColor();
                                        }
                                        else
                                        {
                                            Console.ForegroundColor = ConsoleColor.Red;
                                            Console.WriteLine("\nStudent is already in the group.\n");
                                            Console.ResetColor();
                                        }
                                    }
                                    else
                                    {
                                        Console.ForegroundColor = ConsoleColor.Red;
                                        Console.WriteLine($"\nGroup with ID {groupId} not found.\n");
                                        Console.ResetColor();
                                    }
                                }
                                else
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("\nInvalid group ID format.\n");
                                    Console.ResetColor();
                                }
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine($"\nStudent with ID {studentId} not found.\n");
                                Console.ResetColor();
                            }
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("\nInvalid student ID format.\n");
                            Console.ResetColor();
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(ex.Message);
                        Console.ResetColor();
                        goto case (int)Menu.AddSt;
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


