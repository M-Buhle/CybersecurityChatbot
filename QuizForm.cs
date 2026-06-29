using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
namespace CybersecurityChatbot
{
   public class QuizForm : Form
   {
       // === UI Controls ===
       private Label questionLabel;
       private Button optionA;
       private Button optionB;
       private Button optionC;
       private Button optionD;
       private Label feedbackLabel;
       private Label scoreLabel;
       private Label progressLabel;
       private Panel buttonPanel;
       // === Quiz State ===
       private int currentQuestion = 0;
       private int score = 0;
       private ActivityLog activityLog;
       // === Questions List ===
       private List<QuizQuestion> questions = new List<QuizQuestion>
       {
           new QuizQuestion("What does HTTPS stand for?",
               new[] {"HyperText Transfer Protocol Secure", "High Transfer Protocol System", "HyperText Transfer Process Secure", "High Text Protocol Secure"},
               0, "HTTPS means HyperText Transfer Protocol Secure — it encrypts data between your browser and the website."),
           new QuizQuestion("What is phishing?",
               new[] {"A type of malware", "A fake email or message to steal your info", "A firewall setting", "A strong password technique"},
               1, "Phishing is when criminals send fake emails pretending to be trusted organisations to steal your personal information."),
           new QuizQuestion("True or False: You should use the same password for all accounts.",
               new[] {"True", "False", "Sometimes", "Only for important accounts"},
               1, "False! Using the same password everywhere means if one account is hacked, all your accounts are at risk."),
           new QuizQuestion("What is two-factor authentication (2FA)?",
               new[] {"A type of antivirus", "Using two passwords", "An extra security layer beyond just a password", "A firewall setting"},
               2, "2FA adds an extra layer of security by requiring a second verification step beyond just your password."),
           new QuizQuestion("Which of these is a strong password?",
               new[] {"password123", "John1990", "Tr0ub4dor&3", "abc123"},
               2, "A strong password uses a mix of uppercase, lowercase, numbers and symbols and is not a common word."),
           new QuizQuestion("What should you do if you receive a suspicious email?",
               new[] {"Click the link to check it", "Reply asking who they are", "Delete it and report it as phishing", "Forward it to friends"},
               2, "Always delete suspicious emails and report them as phishing. Never click links in unexpected emails."),
           new QuizQuestion("True or False: Public WiFi is always safe to use.",
               new[] {"True", "False", "Only in coffee shops", "Only with a VPN"},
               1, "False! Public WiFi is not secure. Always use a VPN when connecting to public WiFi networks."),
           new QuizQuestion("What does a VPN do?",
               new[] {"Speeds up your internet", "Encrypts your internet connection", "Blocks all websites", "Removes viruses"},
               1, "A VPN encrypts your internet connection, keeping your data safe especially on public WiFi."),
           new QuizQuestion("What is ransomware?",
               new[] {"Software that speeds up your PC", "Malware that locks your files and demands payment", "A type of firewall", "An antivirus program"},
               1, "Ransomware is malicious software that encrypts your files and demands payment to unlock them."),
           new QuizQuestion("Which of these helps protect your privacy online?",
               new[] {"Sharing your location always", "Using private browsing mode", "Using the same email for everything", "Disabling your firewall"},
               1, "Private browsing mode helps protect your privacy by not saving your browsing history or cookies."),
           new QuizQuestion("True or False: Antivirus software alone is enough to protect you online.",
               new[] {"True", "False", "Only on Windows", "Only with updates"},
               1, "False! Antivirus is important but you also need strong passwords, 2FA, and safe browsing habits."),
           new QuizQuestion("What is social engineering in cybersecurity?",
               new[] {"Building social media profiles", "Manipulating people to reveal confidential info", "Writing code for social apps", "A type of encryption"},
               1, "Social engineering tricks people into revealing sensitive information by exploiting trust and human psychology.")
       };
       public QuizForm(ActivityLog log)
       {
           activityLog = log;
           InitializeComponents();
           LoadQuestion();
           activityLog.AddEntry("Quiz started");
       }
       private void InitializeComponents()
       {
           this.Text = "Cybersecurity Quiz";
           this.Size = new Size(700, 500);
           this.BackColor = Color.FromArgb(13, 17, 23);
           this.StartPosition = FormStartPosition.CenterScreen;
           this.FormBorderStyle = FormBorderStyle.FixedSingle;
           this.MaximizeBox = false;
           this.Font = new Font("Consolas", 10);
           // Progress label
           progressLabel = new Label();
           progressLabel.Location = new Point(20, 15);
           progressLabel.Size = new Size(650, 25);
           progressLabel.ForeColor = Color.Gray;
           progressLabel.Font = new Font("Consolas", 9);
           // Score label
           scoreLabel = new Label();
           scoreLabel.Location = new Point(20, 40);
           scoreLabel.Size = new Size(650, 25);
           scoreLabel.ForeColor = Color.LimeGreen;
           scoreLabel.Font = new Font("Consolas", 9);
           // Question label
           questionLabel = new Label();
           questionLabel.Location = new Point(20, 80);
           questionLabel.Size = new Size(650, 80);
           questionLabel.ForeColor = Color.Cyan;
           questionLabel.Font = new Font("Consolas", 11, FontStyle.Bold);
           questionLabel.AutoSize = false;
           // Button panel
           buttonPanel = new Panel();
           buttonPanel.Location = new Point(20, 170);
           buttonPanel.Size = new Size(650, 200);
           buttonPanel.BackColor = Color.Transparent;
           // Option buttons
           optionA = CreateOptionButton("A", 0);
           optionB = CreateOptionButton("B", 1);
           optionC = CreateOptionButton("C", 2);
           optionD = CreateOptionButton("D", 3);
           buttonPanel.Controls.AddRange(new Control[] { optionA, optionB, optionC, optionD });
           // Feedback label
           feedbackLabel = new Label();
           feedbackLabel.Location = new Point(20, 380);
           feedbackLabel.Size = new Size(650, 80);
           feedbackLabel.ForeColor = Color.Yellow;
           feedbackLabel.Font = new Font("Consolas", 9);
           feedbackLabel.AutoSize = false;
           this.Controls.AddRange(new Control[] {
               progressLabel, scoreLabel, questionLabel,
               buttonPanel, feedbackLabel
           });
       }
       private Button CreateOptionButton(string prefix, int index)
       {
           Button btn = new Button();
           btn.Size = new Size(620, 40);
           btn.Location = new Point(0, index * 48);
           btn.BackColor = Color.FromArgb(22, 27, 34);
           btn.ForeColor = Color.White;
           btn.FlatStyle = FlatStyle.Flat;
           btn.FlatAppearance.BorderColor = Color.FromArgb(0, 150, 136);
           btn.TextAlign = ContentAlignment.MiddleLeft;
           btn.Font = new Font("Consolas", 10);
           btn.Tag = index;
           btn.Click += OptionButton_Click;
           return btn;
       }
       private void LoadQuestion()
       {
           if (currentQuestion >= questions.Count)
           {
               ShowFinalScore();
               return;
           }
           QuizQuestion q = questions[currentQuestion];
           progressLabel.Text = $"Question {currentQuestion + 1} of {questions.Count}";
           scoreLabel.Text = $"Score: {score}/{currentQuestion}";
           questionLabel.Text = q.Question;
           feedbackLabel.Text = "";
           Button[] buttons = { optionA, optionB, optionC, optionD };
           for (int i = 0; i < buttons.Length; i++)
           {
               buttons[i].Text = $"  {(char)('A' + i)}) {q.Options[i]}";
               buttons[i].BackColor = Color.FromArgb(22, 27, 34);
               buttons[i].Enabled = true;
           }
       }
       private void OptionButton_Click(object sender, EventArgs e)
       {
           Button clicked = (Button)sender;
           int selected = (int)clicked.Tag;
           QuizQuestion q = questions[currentQuestion];
           Button[] buttons = { optionA, optionB, optionC, optionD };
           foreach (Button btn in buttons)
               btn.Enabled = false;
           if (selected == q.CorrectIndex)
           {
               score++;
               clicked.BackColor = Color.FromArgb(0, 100, 0);
               feedbackLabel.ForeColor = Color.LimeGreen;
               feedbackLabel.Text = "Correct! " + q.Explanation;
               activityLog.AddEntry($"Quiz: Answered question {currentQuestion + 1} correctly");
           }
           else
           {
               clicked.BackColor = Color.FromArgb(139, 0, 0);
               buttons[q.CorrectIndex].BackColor = Color.FromArgb(0, 100, 0);
               feedbackLabel.ForeColor = Color.OrangeRed;
               feedbackLabel.Text = $"Incorrect! The answer was {(char)('A' + q.CorrectIndex)}. {q.Explanation}";
               activityLog.AddEntry($"Quiz: Answered question {currentQuestion + 1} incorrectly");
           }
           currentQuestion++;
           // Auto advance after 3 seconds
           System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
           timer.Interval = 3000;
           timer.Tick += (s, ev) =>
           {
               timer.Stop();
               LoadQuestion();
           };
           timer.Start();
       }
       private void ShowFinalScore()
       {
           questionLabel.Text = "Quiz Complete!";
           feedbackLabel.ForeColor = Color.Cyan;
           scoreLabel.Text = $"Final Score: {score}/{questions.Count}";
           string feedback;
           if (score >= 10)
               feedback = "Outstanding! You are a cybersecurity pro!";
           else if (score >= 7)
               feedback = "Great job! You have good cybersecurity knowledge!";
           else if (score >= 5)
               feedback = "Not bad! Keep learning to stay safe online!";
           else
               feedback = "Keep practicing! Cybersecurity knowledge is important!";
           feedbackLabel.Text = feedback;
           activityLog.AddEntry($"Quiz completed - Score: {score}/{questions.Count}");
           buttonPanel.Visible = false;
           progressLabel.Text = "Quiz finished!";
           Button closeBtn = new Button();
           closeBtn.Text = "Close Quiz";
           closeBtn.Location = new Point(200, 200);
           closeBtn.Size = new Size(200, 40);
           closeBtn.BackColor = Color.FromArgb(0, 150, 136);
           closeBtn.ForeColor = Color.White;
           closeBtn.FlatStyle = FlatStyle.Flat;
           closeBtn.Click += (s, e) => this.Close();
           this.Controls.Add(closeBtn);
       }
   }
   public class QuizQuestion
   {
       public string Question { get; set; }
       public string[] Options { get; set; }
       public int CorrectIndex { get; set; }
       public string Explanation { get; set; }
       public QuizQuestion(string question, string[] options, int correctIndex, string explanation)
       {
           Question = question;
           Options = options;
           CorrectIndex = correctIndex;
           Explanation = explanation;
       }
   }
}