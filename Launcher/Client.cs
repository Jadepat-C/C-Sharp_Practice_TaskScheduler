using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TaskScheduler.BusinessLogic;
using TaskScheduler.TransferObjects;

namespace TaskScheduler.Launcher
{
    internal class Client
    {
        private bool retry = false;
        private TaskLogic taskLogic = new TaskLogic();
        public Client() { }
        public void MainMenu()
        {
            Console.Clear();
            Console.WriteLine("Welcome to Task Scheduler");
            Console.WriteLine("1. View all task");
            Console.WriteLine("2. Add new task");
            Console.WriteLine("3. Edit task");
            Console.WriteLine("4. Delete task");
            Console.WriteLine("Please select menu or 'q' to exit the program");
            SelectMenu();
        }

        public void SelectMenu()
        {
            string selectedMenu = Console.ReadLine();
            do
            {
                retry = false;
                switch (selectedMenu)
                {
                    case "1":
                        ViewTaskMenu();
                        Continue();
                        break;
                    case "2":
                        AddTaskMenu();
                        Continue();
                        break;
                    case "3":
                        EditTaskMenu();
                        Continue();
                        break;
                    case "4":
                        DeleteTaskMenu();
                        Continue();
                        break;
                    case "q":
                        do
                        {
                            retry = false;
                            Console.WriteLine("Do you want to exit program? (Y/N)");
                            if (Console.ReadLine().Equals("y", StringComparison.InvariantCultureIgnoreCase) ||
                                Console.ReadLine().Equals("yes", StringComparison.InvariantCultureIgnoreCase))
                            {
                                Environment.Exit(0);
                                break;
                            }
                            else if (Console.ReadLine().Equals("n", StringComparison.InvariantCultureIgnoreCase) ||
                                Console.ReadLine().Equals("no", StringComparison.InvariantCultureIgnoreCase))
                            {
                                MainMenu();
                            }
                            else
                            {
                                Console.WriteLine("Invalid input, please try again");
                                retry = true; //force to loop to prompt user again
                            }
                        } while (retry == true);
                        break;
                    default:
                        retry = true; //force to loop to prompt user again
                        Console.WriteLine("Invalid input, please try again");
                        Console.ReadKey();
                        break;
                }
            }
            while (retry == true);
        }

        public void Continue()
        {
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            MainMenu();
        }

        public void ViewTaskMenu()
        {
            List<TaskDTO>? tasks = taskLogic.GetAllTasks();

            Console.Clear();
            var tableHeader = string.Format($"|{"id",-4}|{"Name",-20}|{"Type",-10}|{"Due Date",-11}|{"Status",-15}|{"Notes",-40}|");
            if (tasks.Count == 0)
            {
                Console.WriteLine("There is no task at the moment");
            }
            else
            {
                Console.WriteLine(tableHeader);
                string dash = "";
                for (int i = 0; i < 107; i++)
                {
                    dash += "-";
                }
                Console.WriteLine(dash);
                for (int i = 0; i < tasks.Count; i++)
                {
                    string dueDate = tasks[i].Due.HasValue ? tasks[i].Due.Value.ToString("yyyy/MM/dd") : "";
                    string taskOutput = string.Format($"|{i + 1,-4}|{tasks[i].Name,-20}|{tasks[i].Type,-10}|{dueDate,-11}|{tasks[i].Status,-15}|{tasks[i].Notes,-40}|");
                    Console.WriteLine(taskOutput);
                }

                Console.WriteLine(dash);
            }
        }

        public void AddTaskMenu()
        {
            TaskDTO task = new TaskDTO();
            string name, type, notes;
            DateTime due;
            do
            {
                retry = false;
                Console.Clear();
                Console.WriteLine("Please enter your task name");
                name = Console.ReadLine();
                try
                {
                    taskLogic.ValidateName(name);
                    task.Name = name;
                }
                catch (ValidationException ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("Press any key to retry...");
                    Console.ReadKey();
                    retry = true;
                    continue;
                }

            } while (retry == true);

            do
            {
                retry = false;
                Console.Clear();
                Console.WriteLine("Please enter your task type");
                type = Console.ReadLine();
                try
                {
                    taskLogic.ValidateType(type);
                    task.Type = type;
                }
                catch (ValidationException ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("Press any key to retry...");
                    Console.ReadKey();
                    retry = true;
                    continue;
                }
            } while (retry == true);

            do
            {
                retry = false;
                Console.Clear();
                Console.WriteLine("Please enter your task due date (Format: yyyy/mm/dd)");
                string dueInput = Console.ReadLine();
                try
                {
                    taskLogic.ValidateDateTime(dueInput, true);
                    if (!string.IsNullOrEmpty(dueInput))
                    {
                        DateTime.TryParse(dueInput.ToString(), out due);
                        task.Due = due;
                    }

                }
                catch (ValidationException ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("Press any key to retry...");
                    Console.ReadKey();
                    retry = true;
                    continue;
                }
            } while (retry == true);

            string status = "IN PROGRESS";
            task.Status = status;

            do
            {
                retry = false;
                Console.Clear();
                Console.WriteLine("Please enter your task notes");
                notes = Console.ReadLine();
                try
                {
                    taskLogic.ValidateNotes(notes);
                    task.Notes = notes;
                }
                catch (ValidationException ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("Press any key to retry...");
                    Console.ReadKey();
                    retry = true;
                    continue;
                }
            } while (retry == true);
            taskLogic.CreateTask(task);
            Console.WriteLine("Task created");
        }

        public void EditTaskMenu()
        {
            int id;
            string name, type, notes, status;
            DateTime due;

            List<TaskDTO>? tasks = taskLogic.GetAllTasks();

            do
            {
                retry = false;
                Console.Clear();
                ViewTaskMenu();
                Console.WriteLine("Please select task to edit");
                string choiceStr = Console.ReadLine();
                int choice;
                if (int.TryParse(choiceStr, out choice))
                {
                    TaskDTO task = tasks[choice - 1];
                    TaskDTO newTask = new TaskDTO
                    {
                        Id = task.Id,
                        Name = task.Name,
                        Type = task.Type,
                        Due = task.Due,
                        Status = task.Status,
                        Notes = task.Notes
                    };
                    do
                    {
                        retry = false;
                        Console.Clear();
                        Console.WriteLine($"Please enter your task name, keep it empty will use current name ({task.Name})");
                        name = Console.ReadLine();
                        if (string.IsNullOrEmpty(name.Trim()))
                        {
                            newTask.Name = task.Name;
                        }
                        else
                        {
                            try
                            {
                                taskLogic.ValidateName(name);
                                newTask.Name = name;
                            }
                            catch (ValidationException ex)
                            {
                                Console.WriteLine(ex.Message);
                                Console.WriteLine("Press any key to retry...");
                                Console.ReadKey();
                                retry = true;
                                continue;
                            }
                        }
                    } while (retry == true);

                    do
                    {
                        retry = false;
                        Console.Clear();
                        Console.WriteLine($"Please enter your task type, keep it empty will use current Type ({task.Type})");
                        type = Console.ReadLine();
                        if (string.IsNullOrEmpty(type.Trim()))
                        {
                            newTask.Type = task.Type;
                        }
                        else
                        {
                            try
                            {
                                taskLogic.ValidateType(type);
                                newTask.Type = type;
                            }
                            catch (ValidationException ex)
                            {
                                Console.WriteLine(ex.Message);
                                Console.WriteLine("Press any key to retry...");
                                Console.ReadKey();
                                retry = true;
                                continue;
                            }
                        }
                    } while (retry == true);

                    do
                    {
                        string dueDate = task.Due.HasValue ? task.Due.Value.ToString("yyyy/MM/dd") : "";
                        retry = false;
                        Console.Clear();
                        Console.WriteLine($"Please enter your task due date (Format: yyyy/mm/dd), keep it empty will use current date ({dueDate})");
                        string dueInput = Console.ReadLine();
                        if (string.IsNullOrEmpty(dueInput.Trim()))
                        {
                            taskLogic.ValidateDateTime(dueInput, true);
                            newTask.Due = task.Due;
                        }
                        else
                        {
                            try
                            {
                                taskLogic.ValidateDateTime(dueInput, true);
                                if (!string.IsNullOrEmpty(dueInput))
                                {
                                    DateTime.TryParse(dueInput.ToString(), out due);
                                    newTask.Due = due;
                                }

                            }
                            catch (ValidationException ex)
                            {
                                Console.WriteLine(ex.Message);
                                Console.WriteLine("Press any key to retry...");
                                Console.ReadKey();
                                retry = true;
                                continue;
                            }
                        }

                    } while (retry == true);

                    do
                    {
                        retry = false;
                        Console.Clear();
                        Console.WriteLine($"Please enter your task type, keep it empty will use current status ({task.Status})");
                        status = Console.ReadLine();
                        if (string.IsNullOrEmpty(status.Trim()))
                        {
                            newTask.Status = task.Status;
                        }
                        else
                        {
                            try
                            {
                                taskLogic.ValidateStatus(status);
                                newTask.Status = status;
                            }
                            catch (ValidationException ex)
                            {
                                Console.WriteLine(ex.Message);
                                Console.WriteLine("Press any key to retry...");
                                Console.ReadKey();
                                retry = true;
                                continue;
                            }
                        }
                    } while (retry == true);

                    do
                    {
                        retry = false;
                        Console.Clear();
                        Console.WriteLine($"Please enter your task notes, keep it empty will use current notes ({task.Notes})");
                        notes = Console.ReadLine();
                        if (string.IsNullOrEmpty(notes.Trim()))
                        {
                            newTask.Notes = task.Notes;
                        }
                        else
                        {
                            try
                            {
                                taskLogic.ValidateNotes(notes);
                                newTask.Notes = notes;
                            }
                            catch (ValidationException ex)
                            {
                                Console.WriteLine(ex.Message);
                                Console.WriteLine("Press any key to retry...");
                                Console.ReadKey();
                                retry = true;
                                continue;
                            }
                        }

                    } while (retry == true);

                    taskLogic.UpdateTask(newTask);
                    Console.WriteLine("Task edited");
                }
                else
                {
                    Console.WriteLine("Invalid input, please try again...");
                    Console.ReadKey();
                    retry = true;
                }
            } while (retry == true);

        }

        public void DeleteTaskMenu()
        {
            List<TaskDTO>? tasks = taskLogic.GetAllTasks();

            do
            {
                retry = false;
                Console.Clear();
                ViewTaskMenu();
                Console.WriteLine("Please select task to delete");
                string choiceStr = Console.ReadLine();
                int choice;
                if (int.TryParse(choiceStr, out choice))
                {
                    TaskDTO task = tasks[choice - 1];
                    do
                    {
                        retry = false;
                        Console.WriteLine($"Do you want to delete {task.Name}? You cannot undo this action (Y/N)");
                        if (Console.ReadLine().Equals("y", StringComparison.InvariantCultureIgnoreCase) ||
                            Console.ReadLine().Equals("yes", StringComparison.InvariantCultureIgnoreCase))
                        {
                            taskLogic.DeleteTask(task);
                            Console.WriteLine("Task deleted");
                        }
                        else if (Console.ReadLine().Equals("n", StringComparison.InvariantCultureIgnoreCase) ||
                            Console.ReadLine().Equals("no", StringComparison.InvariantCultureIgnoreCase))
                        {
                            MainMenu();
                        }
                        else
                        {
                            Console.WriteLine("Invalid input, please try again");
                            retry = true; //force to loop to prompt user again
                        }
                    } while (retry == true);
                }
                else
                {
                    Console.WriteLine("Invalid input, please try again...");
                    Console.ReadKey();
                    retry = true;
                }
            } while (retry == true);

        }
    }
}
