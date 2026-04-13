using System;
using System.IO;
using System.Threading;
using NAudio.Wave;
using CybersecurityChatbot.Responses;
class Program
{
   static void Main(string[] args)
   {
       // Play voice greeting when app launches
       PlayVoiceGreeting();
       // Display ASCII art logo
       DisplayAsciiLogo();
       // Ask for the user's name and greet them
       Console.ForegroundColor = ConsoleColor.Cyan;
       Console.Write("\n  Please enter your name: ");
       Console.ResetColor();
       string userName = Console.ReadLine();
       // Personalised welcome using their name
       TypeText($"\n  Welcome, {userName}! I am your Cybersecurity Awareness Assistant.\n");
       // Create the response system
       ChatbotResponses responder = new ChatbotResponses();
       Console.ForegroundColor = ConsoleColor.Yellow;
       TypeText("  Type your question below. Type 'exit' to quit.\n");
       Console.ResetColor();
       PrintDivider();
       // Keep chatting until user types exit
       while (true)
       {
           Console.ForegroundColor = ConsoleColor.Green;
           Console.Write($"\n  {userName}: ");
           Console.ResetColor();
           string userInput = Console.ReadLine();
           // Allow the user to exit
           if (userInput.ToLower() == "exit")
           {
               TypeText($"\n  Goodbye, {userName}! Stay safe online!\n");
               break;
           }
           string response = responder.GetResponse(userInput);
           Console.ForegroundColor = ConsoleColor.Magenta;
           TypeText($"\n  Bot: {response}\n");
           Console.ResetColor();
           PrintDivider();
       }
   }
   // METHOD: Play the WAV voice greeting
   static void PlayVoiceGreeting()
   {
       string audioPath = "Audio/greeting.wav";
       if (File.Exists(audioPath))
       {
           using var audioFile = new AudioFileReader(audioPath);
           using var outputDevice = new WaveOutEvent();
           outputDevice.Init(audioFile);
           outputDevice.Play();
           while (outputDevice.PlaybackState == PlaybackState.Playing)
           {
               Thread.Sleep(100);
           }
       }
       else
       {
           Console.ForegroundColor = ConsoleColor.Red;
           Console.WriteLine("  [Voice greeting file not found. Place greeting.wav in the Audio folder.]");
           Console.ResetColor();
       }
   }
   // METHOD: Display ASCII Art Logo
   static void DisplayAsciiLogo()
   {
       Console.ForegroundColor = ConsoleColor.Cyan;
       Console.WriteLine(@"
 ╔══════════════════════════════════════════════════╗
 ║   ____      _                                    ║
 ║  / ___|   _| |__   ___ _ __                      ║
 ║ | |  | | | | '_ \ / _ \ '__|                     ║
 ║ | |__| |_| | |_) |  __/ |                        ║
 ║  \____\__, |_.__/ \___|_|                        ║
 ║       |___/  Awareness Bot                       ║
 ║                                                  ║
 ║     Your guide to staying safe online!           ║
 ╚══════════════════════════════════════════════════╝
       ");
       Console.ResetColor();
   }
   // METHOD: Typing effect for a conversational feel
   static void TypeText(string text)
   {
       foreach (char c in text)
       {
           Console.Write(c);
           Thread.Sleep(18);
       }
   }
   // METHOD: Print a visual divider for structure
   static void PrintDivider()
   {
       Console.ForegroundColor = ConsoleColor.DarkGray;
       Console.WriteLine("  --------------------------------------------------");
       Console.ResetColor();
   }
}