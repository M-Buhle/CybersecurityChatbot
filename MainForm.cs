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
       private Label titleLabel;
       private Label asciiLabel;
       // === Memory and State ===
       private string userName = "";
       private string favouriteTopic = "";
       private string lastTopic = "";
       private bool nameAsked = false;
       // === Response System ===
       private ChatbotResponses responder;
       public MainForm()
       {
           responder = new ChatbotResponses();
           InitializeComponents();
           PlayVoiceGreeting();
           StartConversation();
       }
       private void InitializeComponents()
       {
           // === Form Setup ===
           this.Text = "Cybersecurity Awareness Bot";
           this.Size = new Size(850, 650);
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
           asciiLabel.Size = new Size(820, 100);
           asciiLabel.AutoSize = false;
           // === Title Label ===
           titleLabel = new Label();
           titleLabel.Text = "Your guide to staying safe online!";
           titleLabel.ForeColor = Color.LimeGreen;
           titleLabel.BackColor = Color.Transparent;
           titleLabel.Font = new Font("Consolas", 11, FontStyle.Bold);
           titleLabel.Location = new Point(10, 115);
           titleLabel.Size = new Size(820, 25);
           titleLabel.TextAlign = ContentAlignment.MiddleCenter;
           // === Chat Display ===
           chatDisplay = new RichTextBox();
           chatDisplay.Location = new Point(10, 145);
           chatDisplay.Size = new Size(815, 390);
           chatDisplay.BackColor = Color.FromArgb(13, 17, 23);
           chatDisplay.ForeColor = Color.White;
           chatDisplay.Font = new Font("Consolas", 10);
           chatDisplay.ReadOnly = true;
           chatDisplay.BorderStyle = BorderStyle.FixedSingle;
           chatDisplay.ScrollBars = RichTextBoxScrollBars.Vertical;
           // === User Input Box ===
           userInput = new TextBox();
           userInput.Location = new Point(10, 545);
           userInput.Size = new Size(680, 30);
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
           sendButton.Location = new Point(700, 543);
           sendButton.Size = new Size(125, 32);
           sendButton.BackColor = Color.FromArgb(0, 150, 136);
           sendButton.ForeColor = Color.White;
           sendButton.Font = new Font("Consolas", 10, FontStyle.Bold);
           sendButton.FlatStyle = FlatStyle.Flat;
           sendButton.FlatAppearance.BorderSize = 0;
           sendButton.Click += (s, e) => SendMessage();
           // === Sentiment Status Label ===
           Label sentimentLabel = new Label();
           sentimentLabel.Name = "sentimentLabel";
           sentimentLabel.Text = "Mood: Neutral";
           sentimentLabel.ForeColor = Color.Gray;
           sentimentLabel.BackColor = Color.Transparent;
           sentimentLabel.Font = new Font("Consolas", 9);
           sentimentLabel.Location = new Point(10, 580);
           sentimentLabel.Size = new Size(200, 20);
           this.Controls.Add(sentimentLabel);
           // === Add controls to form ===
           this.Controls.Add(asciiLabel);
           this.Controls.Add(titleLabel);
           this.Controls.Add(chatDisplay);
           this.Controls.Add(userInput);
           this.Controls.Add(sendButton);
       }
       private void StartConversation()
       {
           AppendMessage("Bot", "Hello! Welcome to the Cybersecurity Awareness Bot!", Color.Cyan);
           AppendMessage("Bot", "What is your name?", Color.Cyan);
           nameAsked = true;
       }
       private void SendMessage()
       {
           string input = userInput.Text.Trim();
           if (string.IsNullOrWhiteSpace(input)) return;
           AppendMessage("You", input, Color.LimeGreen);
           userInput.Clear();
           // === Step 1: Get the user's name first ===
           if (nameAsked && string.IsNullOrEmpty(userName))
           {
               userName = input;
               nameAsked = false;
               AppendMessage("Bot", $"Nice to meet you, {userName}! I am your Cybersecurity Awareness Assistant.", Color.Cyan);
               AppendMessage("Bot", $"You can ask me about passwords, phishing, malware, VPN, privacy, scams, or safe browsing.", Color.Cyan);
               return;
           }
           // === Step 2: Process the message ===
           ProcessInput(input);
       }
       private void ProcessInput(string input)
       {
           string lower = input.ToLower();
           // === Sentiment Detection ===
           if (lower.Contains("worried") || lower.Contains("scared") || lower.Contains("anxious"))
           {
               AppendMessage("Bot", $"I understand you feel worried, {userName}. That is completely normal! Cybersecurity can feel overwhelming but I am here to help you.", Color.Yellow);
               AppendMessage("Bot", "Let me share a tip to help you feel safer: Always keep your software updated — this closes security gaps that hackers exploit.", Color.Cyan);
               return;
           }
           if (lower.Contains("frustrated") || lower.Contains("confused") || lower.Contains("dont understand"))
           {
               AppendMessage("Bot", $"No worries, {userName}! Let me explain more clearly. Feel free to ask me anything and I will break it down simply.", Color.Yellow);
               return;
           }
           if (lower.Contains("curious") || lower.Contains("interested") || lower.Contains("want to know"))
           {
               AppendMessage("Bot", $"I love your curiosity, {userName}! Staying informed is the best defence against cyber threats.", Color.Yellow);
           }
           // === Conversation Flow ===
           if (lower.Contains("tell me more") || lower.Contains("explain more") || lower.Contains("give me another tip") || lower.Contains("more info"))
           {
               if (!string.IsNullOrEmpty(lastTopic))
               {
                   string followUp = responder.GetFollowUp(lastTopic);
                   AppendMessage("Bot", followUp, Color.Cyan);
                   return;
               }
               else
               {
                   AppendMessage("Bot", "What topic would you like to know more about? Try asking about passwords, phishing, or malware!", Color.Cyan);
                   return;
               }
           }
           // === Memory: Remember favourite topic ===
           if (lower.Contains("interested in") || lower.Contains("i like") || lower.Contains("favourite topic"))
           {
               favouriteTopic = input;
               AppendMessage("Bot", $"Got it, {userName}! I will remember that. {input} is a great area to focus on for staying safe online!", Color.Cyan);
               return;
           }
           // === Get response from ChatbotResponses ===
           string response = responder.GetResponse(input, userName);
           AppendMessage("Bot", response, Color.Cyan);
           // === Update last topic for conversation flow ===
           if (lower.Contains("password")) lastTopic = "password";
           else if (lower.Contains("phishing")) lastTopic = "phishing";
           else if (lower.Contains("malware")) lastTopic = "malware";
           else if (lower.Contains("vpn")) lastTopic = "vpn";
           else if (lower.Contains("privacy")) lastTopic = "privacy";
           else if (lower.Contains("scam")) lastTopic = "scam";
           else if (lower.Contains("browsing")) lastTopic = "browsing";
           // === Recall favourite topic occasionally ===
           if (!string.IsNullOrEmpty(favouriteTopic) && response.Length > 10)
           {
               AppendMessage("Bot", $"By the way, since you are interested in {favouriteTopic}, make sure to also check your account security settings regularly!", Color.Magenta);
           }
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