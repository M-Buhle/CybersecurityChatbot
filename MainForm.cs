using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using NAudio.Wave;
using CybersecurityChatbot.Responses;
namespace CybersecurityChatbot
{
   public class MainForm : Form
   {
       // === UI Controls ===
       private RichTextBox chatDisplay;
       private TextBox userInput;
       private Button sendButton;
       private Button quizButton;
       private Button tasksButton;
       private Label titleLabel;
       private Label asciiLabel;
       // === Managers ===
       private ChatbotResponses responder;
       private TaskManager taskManager;
       private ActivityLog activityLog;
       // === Memory and State ===
       private string userName = "";
       private string favouriteTopic = "";
       private string lastTopic = "";
       private bool nameAsked = false;
       private bool awaitingTaskTitle = false;
       private bool awaitingTaskDescription = false;
       private bool awaitingTaskReminder = false;
       private string pendingTaskTitle = "";
       private string pendingTaskDescription = "";
       public MainForm()
       {
           responder = new ChatbotResponses();
           taskManager = new TaskManager();
           activityLog = new ActivityLog();
           activityLog.LoadLog();
           InitializeComponents();
           PlayVoiceGreeting();
           StartConversation();
       }
       private void InitializeComponents()
       {
           this.Text = "Cybersecurity Awareness Bot";
           this.Size = new Size(900, 700);
           this.BackColor = Color.FromArgb(13, 17, 23);
           this.FormBorderStyle = FormBorderStyle.FixedSingle;
           this.MaximizeBox = false;
           this.StartPosition = FormStartPosition.CenterScreen;
           this.Font = new Font("Consolas", 10);
           // === ASCII Art Label ===
           asciiLabel = new Label();
           asciiLabel.Text = "██████╗██╗   ██╗██████╗ ███████╗██████╗     ██████╗  ██████╗ ████████╗\r\n" +
                             "██╔════╝╚██╗ ██╔╝██╔══██╗██╔════╝██╔══██╗    ██╔══██╗██╔═══██╗╚══██╔══╝\r\n" +
                             "██║      ╚████╔╝ ██████╔╝█████╗  ██████╔╝    ██████╔╝██║   ██║   ██║   \r\n" +
                             "██║       ╚██╔╝  ██╔══██╗██╔══╝  ██╔══██╗    ██╔══██╗██║   ██║   ██║   \r\n" +
                             "╚██████╗   ██║   ██████╔╝███████╗██║  ██║    ██████╔╝╚██████╔╝   ██║   \r\n" +
                             " ╚═════╝   ╚═╝   ╚═════╝ ╚══════╝╚═╝  ╚═╝    ╚═════╝  ╚═════╝    ╚═╝  ";
           asciiLabel.ForeColor = Color.Cyan;
           asciiLabel.BackColor = Color.Transparent;
           asciiLabel.Font = new Font("Consolas", 7);
           asciiLabel.Location = new Point(10, 10);
           asciiLabel.Size = new Size(870, 100);
           asciiLabel.AutoSize = false;
           // === Title Label ===
           titleLabel = new Label();
           titleLabel.Text = "Your guide to staying safe online!";
           titleLabel.ForeColor = Color.LimeGreen;
           titleLabel.BackColor = Color.Transparent;
           titleLabel.Font = new Font("Consolas", 11, FontStyle.Bold);
           titleLabel.Location = new Point(10, 115);
           titleLabel.Size = new Size(870, 25);
           titleLabel.TextAlign = ContentAlignment.MiddleCenter;
           // === Quiz Button ===
           quizButton = new Button();
           quizButton.Text = "Take Quiz";
           quizButton.Location = new Point(10, 145);
           quizButton.Size = new Size(120, 30);
           quizButton.BackColor = Color.FromArgb(75, 0, 130);
           quizButton.ForeColor = Color.White;
           quizButton.FlatStyle = FlatStyle.Flat;
           quizButton.FlatAppearance.BorderSize = 0;
           quizButton.Font = new Font("Consolas", 9, FontStyle.Bold);
           quizButton.Click += (s, e) =>
           {
               activityLog.AddEntry("User opened the quiz");
               QuizForm quiz = new QuizForm(activityLog);
               quiz.ShowDialog();
           };
           // === Tasks Button ===
           tasksButton = new Button();
           tasksButton.Text = "View Tasks";
           tasksButton.Location = new Point(140, 145);
           tasksButton.Size = new Size(120, 30);
           tasksButton.BackColor = Color.FromArgb(0, 100, 0);
           tasksButton.ForeColor = Color.White;
           tasksButton.FlatStyle = FlatStyle.Flat;
           tasksButton.FlatAppearance.BorderSize = 0;
           tasksButton.Font = new Font("Consolas", 9, FontStyle.Bold);
           tasksButton.Click += (s, e) =>
           {
               string tasks = taskManager.GetAllTasks();
               AppendMessage("Bot", tasks, Color.Cyan);
               activityLog.AddEntry("User viewed all tasks");
           };
           // === Chat Display ===
           chatDisplay = new RichTextBox();
           chatDisplay.Location = new Point(10, 185);
           chatDisplay.Size = new Size(865, 420);
           chatDisplay.BackColor = Color.FromArgb(13, 17, 23);
           chatDisplay.ForeColor = Color.White;
           chatDisplay.Font = new Font("Consolas", 10);
           chatDisplay.ReadOnly = true;
           chatDisplay.BorderStyle = BorderStyle.FixedSingle;
           chatDisplay.ScrollBars = RichTextBoxScrollBars.Vertical;
           // === User Input Box ===
           userInput = new TextBox();
           userInput.Location = new Point(10, 615);
           userInput.Size = new Size(650, 30);
           userInput.BackColor = Color.FromArgb(22, 27, 34);
           userInput.ForeColor = Color.White;
           userInput.Font = new Font("Consolas", 10);
           userInput.BorderStyle = BorderStyle.FixedSingle;
           userInput.KeyPress += (s, e) =>
           {
               if (e.KeyChar == (char)Keys.Enter)
               {
                   e.Handled = true;
                   SendMessage();
               }
           };
           // === Send Button ===
           sendButton = new Button();
           sendButton.Text = "SEND";
           sendButton.Location = new Point(670, 613);
           sendButton.Size = new Size(205, 32);
           sendButton.BackColor = Color.FromArgb(0, 150, 136);
           sendButton.ForeColor = Color.White;
           sendButton.Font = new Font("Consolas", 10, FontStyle.Bold);
           sendButton.FlatStyle = FlatStyle.Flat;
           sendButton.FlatAppearance.BorderSize = 0;
           sendButton.Click += (s, e) => SendMessage();
           this.Controls.Add(asciiLabel);
           this.Controls.Add(titleLabel);
           this.Controls.Add(quizButton);
           this.Controls.Add(tasksButton);
           this.Controls.Add(chatDisplay);
           this.Controls.Add(userInput);
           this.Controls.Add(sendButton);
       }
       private void StartConversation()
       {
           AppendMessage("Bot", "Hello! Welcome to the Cybersecurity Awareness Bot!", Color.Cyan);
           AppendMessage("Bot", "You can use the buttons above to take a quiz or view your tasks.", Color.Cyan);
           AppendMessage("Bot", "What is your name?", Color.Cyan);
           nameAsked = true;
       }
       private void SendMessage()
       {
           string input = userInput.Text.Trim();
           if (string.IsNullOrWhiteSpace(input)) return;
           AppendMessage("You", input, Color.LimeGreen);
           userInput.Clear();
           // === Get name first ===
           if (nameAsked && string.IsNullOrEmpty(userName))
           {
               userName = input;
               nameAsked = false;
               AppendMessage("Bot", $"Nice to meet you, {userName}! I am your Cybersecurity Awareness Assistant.", Color.Cyan);
               AppendMessage("Bot", "You can ask me about cybersecurity topics, add tasks, take a quiz, or view your activity log!", Color.Cyan);
               AppendMessage("Bot", "Try typing: 'add task', 'view tasks', 'start quiz', 'show activity log', or ask me anything!", Color.Yellow);
               activityLog.AddEntry($"User {userName} started a session");
               return;
           }
           // === Handle task creation flow ===
           if (awaitingTaskTitle)
           {
               pendingTaskTitle = input;
               awaitingTaskTitle = false;
               awaitingTaskDescription = true;
               AppendMessage("Bot", "Great! Now give me a description for this task:", Color.Cyan);
               return;
           }
           if (awaitingTaskDescription)
           {
               pendingTaskDescription = input;
               awaitingTaskDescription = false;
               awaitingTaskReminder = true;
               AppendMessage("Bot", "Would you like to set a reminder? If yes type it (e.g. 'Remind me in 3 days') or type 'no' to skip:", Color.Cyan);
               return;
           }
           if (awaitingTaskReminder)
           {
               string reminder = input.ToLower() == "no" ? "" : input;
               string result = taskManager.AddTask(pendingTaskTitle, pendingTaskDescription, reminder);
               AppendMessage("Bot", result, Color.Cyan);
               activityLog.AddEntry($"Task added: '{pendingTaskTitle}'");
               awaitingTaskReminder = false;
               return;
           }
           ProcessInput(input);
       }
       private void ProcessInput(string input)
       {
           string lower = input.ToLower();
           // === Activity Log ===
           if (lower.Contains("show activity log") || lower.Contains("what have you done") || lower.Contains("activity log"))
           {
               AppendMessage("Bot", activityLog.GetLog(), Color.Cyan);
               activityLog.AddEntry("User viewed activity log");
               return;
           }
           // === NLP: Task Commands ===
           if (lower.Contains("add task") || lower.Contains("new task") || lower.Contains("create task") ||
               lower.Contains("remind me to") || lower.Contains("set a reminder") || lower.Contains("add a task"))
           {
               awaitingTaskTitle = true;
               AppendMessage("Bot", $"Sure {userName}! What is the title of the task you want to add?", Color.Cyan);
               activityLog.AddEntry("User started adding a task");
               return;
           }
           if (lower.Contains("view tasks") || lower.Contains("show tasks") || lower.Contains("my tasks") ||
               lower.Contains("list tasks") || lower.Contains("what are my tasks"))
           {
               AppendMessage("Bot", taskManager.GetAllTasks(), Color.Cyan);
               activityLog.AddEntry("User viewed tasks");
               return;
           }
           if (lower.Contains("complete task") || lower.Contains("mark task") || lower.Contains("finish task"))
           {
               AppendMessage("Bot", "Which task number would you like to mark as complete? Type the number:", Color.Cyan);
               AppendMessage("Bot", taskManager.GetAllTasks(), Color.Cyan);
               return;
           }
           if (lower.Contains("delete task") || lower.Contains("remove task"))
           {
               AppendMessage("Bot", "Which task number would you like to delete? Type the number:", Color.Cyan);
               AppendMessage("Bot", taskManager.GetAllTasks(), Color.Cyan);
               return;
           }
           // Handle task number input for complete/delete
           if (int.TryParse(input.Trim(), out int taskNum))
           {
               string result = taskManager.CompleteTask(taskNum);
               AppendMessage("Bot", result, Color.Cyan);
               activityLog.AddEntry($"Task {taskNum} marked as completed");
               return;
           }
           // === NLP: Quiz Commands ===
           if (lower.Contains("start quiz") || lower.Contains("take quiz") || lower.Contains("quiz") ||
               lower.Contains("test me") || lower.Contains("play game"))
           {
               activityLog.AddEntry("User started quiz via chat");
               QuizForm quiz = new QuizForm(activityLog);
               quiz.ShowDialog();
               return;
           }
           // === Sentiment Detection ===
           if (lower.Contains("worried") || lower.Contains("scared") || lower.Contains("anxious"))
           {
               AppendMessage("Bot", $"I understand you feel worried, {userName}. That is completely normal! Let me share a tip to help you feel safer:", Color.Yellow);
               AppendMessage("Bot", "Always keep your software updated — this closes security gaps that hackers exploit.", Color.Cyan);
               activityLog.AddEntry("Sentiment detected: worried");
               return;
           }
           if (lower.Contains("frustrated") || lower.Contains("confused") || lower.Contains("dont understand"))
           {
               AppendMessage("Bot", $"No worries, {userName}! Let me explain more clearly. Feel free to ask me anything!", Color.Yellow);
               activityLog.AddEntry("Sentiment detected: frustrated");
               return;
           }
           if (lower.Contains("curious") || lower.Contains("interested"))
           {
               AppendMessage("Bot", $"I love your curiosity, {userName}! Staying informed is the best defence against cyber threats.", Color.Yellow);
               activityLog.AddEntry("Sentiment detected: curious");
           }
           // === Conversation Flow ===
           if (lower.Contains("tell me more") || lower.Contains("explain more") ||
               lower.Contains("give me another tip") || lower.Contains("more info"))
           {
               if (!string.IsNullOrEmpty(lastTopic))
               {
                   string followUp = responder.GetFollowUp(lastTopic);
                   AppendMessage("Bot", followUp, Color.Cyan);
                   activityLog.AddEntry($"User asked for more info on {lastTopic}");
                   return;
               }
               AppendMessage("Bot", "What topic would you like to know more about?", Color.Cyan);
               return;
           }
           // === Memory ===
           if (lower.Contains("interested in") || lower.Contains("i like") || lower.Contains("favourite topic"))
           {
               favouriteTopic = input;
               AppendMessage("Bot", $"Got it, {userName}! I will remember that.", Color.Cyan);
               activityLog.AddEntry($"User interest noted: {input}");
               return;
           }
           // === Get cybersecurity response ===
           string response = responder.GetResponse(input, userName);
           AppendMessage("Bot", response, Color.Cyan);
           // Update last topic
           if (lower.Contains("password")) lastTopic = "password";
           else if (lower.Contains("phishing")) lastTopic = "phishing";
           else if (lower.Contains("malware")) lastTopic = "malware";
           else if (lower.Contains("vpn")) lastTopic = "vpn";
           else if (lower.Contains("privacy")) lastTopic = "privacy";
           else if (lower.Contains("scam")) lastTopic = "scam";
           else if (lower.Contains("browsing")) lastTopic = "browsing";
           activityLog.AddEntry($"Bot responded to: {input.Substring(0, Math.Min(30, input.Length))}");
           activityLog.SaveLog();
       }
       private void AppendMessage(string sender, string message, Color color)
       {
           chatDisplay.SelectionStart = chatDisplay.TextLength;
           chatDisplay.SelectionLength = 0;
           chatDisplay.SelectionColor = color;
           chatDisplay.AppendText($"{sender}: {message}\n\n");
           chatDisplay.SelectionColor = chatDisplay.ForeColor;
           chatDisplay.ScrollToCaret();
       }
       private void PlayVoiceGreeting()
       {
           string audioPath = "Audio/greeting.wav";
           if (System.IO.File.Exists(audioPath))
           {
               Thread audioThread = new Thread(() =>
               {
                   using var audioFile = new AudioFileReader(audioPath);
                   using var outputDevice = new WaveOutEvent();
                   outputDevice.Init(audioFile);
                   outputDevice.Play();
                   while (outputDevice.PlaybackState == PlaybackState.Playing)
                       Thread.Sleep(100);
               });
               audioThread.IsBackground = true;
               audioThread.Start();
           }
       }
   }
}