using System;
using System.Collections.Generic;

namespace ToDoList
{
    public class ListMaker
    {
        List<string> taskList = new List<string>() { };

        public void DisplayMenu()
        {
            Console.WriteLine("***Welcome to the To Do List tracker! ***");
            Console.WriteLine("Choose one of the following options: (Add/View/Quit)");
            string choice = Console.ReadLine();

            if (choice.ToLower() == "add")
            {
                AddTask();
            }
            else if (choice.ToLower() == "view")
            {
                ViewTasks();
            }
            else if (choice.ToLower() == "quit")
            {
                Environment.Exit(0);
            }
            else
            {
                Console.WriteLine("Invalid option!");
                DisplayMenu();
            }
        }

        public void AddTask()
        {
            Console.WriteLine("Enter a description for your new task");
            string task = Console.ReadLine();

            taskList.Add(task);
            Console.WriteLine("Successfully added task.");
            DisplayMenu();
        }

        public void ViewTasks()
        {
            if (taskList.Count == 0)
            {
                Console.WriteLine("You don't have any tasks!");
            } else {
                Console.WriteLine("*** Your Current Tasks ***");
                for (int i = 0; i < taskList.Count; i++)
                {
                    Console.WriteLine("Task " + i + ": " + taskList[i]);
                }
            }

            DisplayMenu();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            ListMaker list = new ListMaker();
            list.DisplayMenu();
        }
    }
}
