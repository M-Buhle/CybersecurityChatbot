using System;

using System.Collections.Generic;

using System.IO;

using System.Text.Json;

namespace CybersecurityChatbot

{

    public class ActivityLog

    {

        private List<string> logEntries = new List<string>();

        private const int MaxEntries = 10;

        // Add an entry to the log with timestamp

        public void AddEntry(string action)

        {

            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            string entry = $"[{timestamp}] {action}";

            logEntries.Add(entry);

            // Keep only last 10 entries

            if (logEntries.Count > MaxEntries)

                logEntries.RemoveAt(0);

        }

        // Get the last 5-10 entries as a formatted string

        public string GetLog()

        {

            if (logEntries.Count == 0)

                return "No activity recorded yet.";

            string log = "Here is a summary of recent actions:\n\n";

            for (int i = 0; i < logEntries.Count; i++)

                log += $"{i + 1}. {logEntries[i]}\n";

            return log;

        }

        // Save log to JSON file

        public void SaveLog()

        {

            string json = JsonSerializer.Serialize(logEntries);

            File.WriteAllText("activitylog.json", json);

        }

        // Load log from JSON file

        public void LoadLog()

        {

            if (File.Exists("activitylog.json"))

            {

                string json = File.ReadAllText("activitylog.json");

                logEntries = JsonSerializer.Deserialize<List<string>>(json) ?? new List<string>();

            }

        }

    }

}
 