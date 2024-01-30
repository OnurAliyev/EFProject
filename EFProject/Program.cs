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
                      "4 == Update student group\n" +
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
                        if (String.IsNullOrEmpty(studentName)) throw new ArgumentNullException();
                        Console.Write("Enter student surname: ");
                        string? studentSurname = Console.ReadLine();
                        if (String.IsNullOrEmpty(studentSurname)) throw new ArgumentNullException();
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
                        string? groupName = Console.ReadLine();
                        if (String.IsNullOrEmpty(groupName)) throw new ArgumentNullException();
                        int groupCapacity;
                        do
                        {
                            Console.Write("Enter group capacity (must be greater than 10): ");
                        } while (!int.TryParse(Console.ReadLine(), out groupCapacity) || groupCapacity <= 10);
                        Group? existingGroup = await context.Groups.FirstOrDefaultAsync(g => g.Name == groupName);
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
                            Student? existingStudent = await context.Students.FindAsync(studentId);
                            if (existingStudent is not null)
                            {
                                Console.Write("Enter group ID to add the student to: ");
                                if (int.TryParse(Console.ReadLine(), out int groupId))
                                {
                                    // Verilən id ilə group var ya yox
                                    Group? existingGroup = await context.Groups.FindAsync(groupId);
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
                                        Console.WriteLine($"\nGroup with ID '{groupId}' not found.\n");
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
                                Console.WriteLine($"\nStudent with ID '{studentId}' not found.\n");
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
                case (int)Menu.StUpdate:
                    try
                    {
                        Console.Write("Enter student ID to update group: ");
                        if (int.TryParse(Console.ReadLine(), out int updateStudentId))
                        {
                            // Verilen id ile student var ya yox
                            Student? existingStudent = await context.Students.FindAsync(updateStudentId);
                            if (existingStudent is not null)
                            {
                                Console.Write("Enter new group ID for the student: ");
                                if (int.TryParse(Console.ReadLine(), out int newGroupId))
                                {
                                    // Verilen id ile group var ya yox
                                    Group? existingGroup = await context.Groups.FindAsync(newGroupId);
                                    if (existingGroup is not null)
                                    {
                                        // Student artiq yeni qrupdadirsa
                                        if (!await context.StudentGroups.AnyAsync(sg => sg.StudentId == updateStudentId && sg.GroupId == newGroupId))
                                        {
                                            // Student oldugu groupdan silmek
                                            var currentStudentGroup = await context.StudentGroups.FirstOrDefaultAsync(sg => sg.StudentId == updateStudentId);
                                            if (currentStudentGroup is not null)
                                            {
                                                context.StudentGroups.Remove(currentStudentGroup);
                                            }

                                            // Add the student to the new group
                                            StudentGroup newStudentGroup = new()
                                            {
                                                StudentId = updateStudentId,
                                                GroupId = newGroupId,
                                            };
                                            await context.StudentGroups.AddAsync(newStudentGroup);
                                            await context.SaveChangesAsync();

                                            Console.ForegroundColor = ConsoleColor.Green;
                                            Console.WriteLine("\nStudent group updated successfully!\n");
                                            Console.ResetColor();
                                        }
                                        else
                                        {
                                            Console.ForegroundColor = ConsoleColor.Red;
                                            Console.WriteLine("\nStudent is already in this group.\n");
                                            Console.ResetColor();
                                        }
                                    }
                                    else
                                    {
                                        Console.ForegroundColor = ConsoleColor.Red;
                                        Console.WriteLine($"\nGroup with ID '{newGroupId}' not found.\n");
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
                                Console.WriteLine($"\nStudent with ID '{updateStudentId}' not found.\n");
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
                        goto case (int)Menu.StUpdate;
                    }
                    break;
                case 0:
                    runApp = false;
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("Application closed!\n" +
                                      $"Press any key to close window...");
                    Console.ResetColor();
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


