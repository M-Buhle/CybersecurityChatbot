using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
namespace CybersecurityChatbot
{
   public class CyberTask
   {
       public string Title { get; set; } = "";
       public string Description { get; set; } = "";
       public string Reminder { get; set; } = "";
       public bool IsCompleted { get; set; } = false;
       public DateTime DateAdded { get; set; } = DateTime.Now;
   }
   public class TaskManager
   {
       private List<CyberTask> tasks = new List<CyberTask>();
       private const string FilePath = "tasks.json";
       public TaskManager()
       {
           LoadTasks();
       }
       // Add a new task
       public string AddTask(string title, string description, string reminder = "")
       {
           CyberTask task = new CyberTask
           {
               Title = title,
               Description = description,
               Reminder = reminder,
               IsCompleted = false,
               DateAdded = DateTime.Now
           };
           tasks.Add(task);
           SaveTasks();
           return $"Task added: '{title}'. {(string.IsNullOrEmpty(reminder) ? "No reminder set." : $"Reminder: {reminder}")}";
       }
       // Get all tasks as formatted string
       public string GetAllTasks()
       {
           if (tasks.Count == 0)
               return "You have no tasks yet. Type 'add task' to add one!";
           string result = "Here are your cybersecurity tasks:\n\n";
           for (int i = 0; i < tasks.Count; i++)
           {
               string status = tasks[i].IsCompleted ? "Completed" : "Pending";
               result += $"{i + 1}. [{status}] {tasks[i].Title}\n";
               result += $"   Description: {tasks[i].Description}\n";
               if (!string.IsNullOrEmpty(tasks[i].Reminder))
                   result += $"   Reminder: {tasks[i].Reminder}\n";
               result += "\n";
           }
           return result;
       }
       // Mark task as completed
       public string CompleteTask(int index)
       {
           if (index < 1 || index > tasks.Count)
               return "Invalid task number. Please try again.";
           tasks[index - 1].IsCompleted = true;
           SaveTasks();
           return $"Task '{tasks[index - 1].Title}' marked as completed!";
       }
       // Delete a task
       public string DeleteTask(int index)
       {
           if (index < 1 || index > tasks.Count)
               return "Invalid task number. Please try again.";
           string title = tasks[index - 1].Title;
           tasks.RemoveAt(index - 1);
           SaveTasks();
           return $"Task '{title}' deleted successfully!";
       }
       // Save tasks to JSON file
       private void SaveTasks()
       {
           string json = JsonSerializer.Serialize(tasks);
           File.WriteAllText(FilePath, json);
       }
       // Load tasks from JSON file
       private void LoadTasks()
       {
           if (File.Exists(FilePath))
           {
               string json = File.ReadAllText(FilePath);
               tasks = JsonSerializer.Deserialize<List<CyberTask>>(json) ?? new List<CyberTask>();
           }
       }
       public int TaskCount => tasks.Count;
   }
}