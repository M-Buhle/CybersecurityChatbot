using System;
using System.Collections.Generic;
namespace CybersecurityChatbot.Responses
{
   public class ChatbotResponses
   {
       private Random random = new Random();
       // === Random response lists for each topic ===
       private List<string> passwordResponses = new List<string>
       {
           "Use at least 12 characters mixing letters, numbers and symbols. Never reuse passwords across sites!",
           "Consider using a password manager to generate and store strong unique passwords safely.",
           "Avoid using personal details like your name or birthday in passwords. Hackers guess these easily!",
           "Change your passwords regularly especially after a data breach. Check haveibeenpwned.com to see if your email was leaked."
       };
       private List<string> phishingResponses = new List<string>
       {
           "Phishing emails pretend to be from trusted organisations. Always verify the sender before clicking any link!",
           "Look for spelling mistakes and urgent language in emails — these are common signs of phishing attacks.",
           "Never click links in unexpected emails. Go directly to the website by typing the address in your browser.",
           "If an email asks for your personal information, it is almost certainly a scam. Legitimate companies never do this."
       };
       private List<string> malwareResponses = new List<string>
       {
           "Malware is harmful software that damages your device. Always keep your antivirus updated!",
           "Never download files from untrusted websites. Malware often hides in free software downloads.",
           "Ransomware is a type of malware that locks your files and demands payment. Always back up your data!",
           "Run regular antivirus scans on your device to detect and remove malware before it causes damage."
       };
       private List<string> privacyResponses = new List<string>
       {
           "Review your social media privacy settings regularly. Limit who can see your personal information.",
           "Be careful what you share online. Even small details can be used by cybercriminals to target you.",
           "Use private browsing mode when using shared computers to protect your personal information.",
           "Read privacy policies before signing up for apps and services to understand how your data is used."
       };
       private List<string> scamResponses = new List<string>
       {
           "If something sounds too good to be true online, it probably is. Be very cautious of online offers!",
           "Never send money to someone you have only met online. Romance scams are very common in South Africa.",
           "Scammers often pretend to be from SARS or banks. Always call the official number to verify.",
           "Be suspicious of unsolicited calls asking for your banking details or OTP numbers."
       };
       private List<string> vpnResponses = new List<string>
       {
           "A VPN encrypts your internet connection keeping your data safe especially on public WiFi.",
           "Always use a VPN when connecting to public WiFi in places like coffee shops or airports.",
           "A VPN hides your IP address making it harder for websites and hackers to track your location.",
           "Choose a reputable paid VPN service. Free VPNs often sell your data to third parties."
       };
       private List<string> browsingResponses = new List<string>
       {
           "Always look for HTTPS in the website URL. The S means the connection is secure and encrypted.",
           "Avoid clicking on pop-up ads. They often lead to malicious websites designed to steal your info.",
           "Keep your browser updated to protect against the latest security vulnerabilities.",
           "Use a reputable browser extension like uBlock Origin to block malicious ads and trackers."
       };
       // === Follow-up responses for conversation flow ===
       private Dictionary<string, string> followUpResponses = new Dictionary<string, string>
       {
           { "password", "Another password tip: Enable two-factor authentication (2FA) on all your important accounts for an extra layer of security!" },
           { "phishing", "Another phishing tip: Hover over links before clicking to see the real URL. If it looks suspicious, do not click it!" },
           { "malware", "Another malware tip: Be careful with USB drives from unknown sources. They can contain malware that infects your device automatically!" },
           { "privacy", "Another privacy tip: Regularly delete apps you no longer use. Old apps can still access your data in the background!" },
           { "scam", "Another scam tip: Never share your OTP (One Time Password) with anyone, even if they claim to be from your bank!" },
           { "vpn", "Another VPN tip: Make sure your VPN has a kill switch feature. This cuts your internet if the VPN drops, keeping you protected!" },
           { "browsing", "Another browsing tip: Clear your browser cookies and cache regularly to remove tracking data stored by websites!" }
       };
       // === Main response method ===
       public string GetResponse(string userInput, string userName = "")
       {
           string input = userInput.ToLower();
           string name = string.IsNullOrEmpty(userName) ? "" : $"{userName}, ";
           if (string.IsNullOrWhiteSpace(input))
               return "It looks like you did not type anything. Please ask me a cybersecurity question!";
           else if (input.Contains("password"))
               return $"{name}{passwordResponses[random.Next(passwordResponses.Count)]}";
           else if (input.Contains("phishing"))
               return $"{name}{phishingResponses[random.Next(phishingResponses.Count)]}";
           else if (input.Contains("malware") || input.Contains("virus") || input.Contains("ransomware"))
               return $"{name}{malwareResponses[random.Next(malwareResponses.Count)]}";
           else if (input.Contains("privacy") || input.Contains("personal data"))
               return $"{name}{privacyResponses[random.Next(privacyResponses.Count)]}";
           else if (input.Contains("scam") || input.Contains("fraud"))
               return $"{name}{scamResponses[random.Next(scamResponses.Count)]}";
           else if (input.Contains("vpn"))
               return $"{name}{vpnResponses[random.Next(vpnResponses.Count)]}";
           else if (input.Contains("browsing") || input.Contains("safe browse") || input.Contains("website"))
               return $"{name}{browsingResponses[random.Next(browsingResponses.Count)]}";
           else if (input.Contains("how are you"))
               return $"I am doing great and ready to help you stay safe online, {userName}! What would you like to know?";
           else if (input.Contains("purpose") || input.Contains("what can you do"))
               return $"I am your Cybersecurity Awareness Assistant, {userName}! Ask me about passwords, phishing, malware, privacy, scams, VPN, or safe browsing.";
           else if (input.Contains("what can i ask") || input.Contains("help"))
               return "You can ask me about: passwords, phishing, malware, privacy, scams, VPN, or safe browsing!";
           else if (input.Contains("thank"))
               return $"You are welcome, {userName}! Stay safe online!";
           else if (input.Contains("bye") || input.Contains("exit") || input.Contains("goodbye"))
               return $"Goodbye, {userName}! Remember to stay safe online!";
           else
               return $"I am not sure I understand that, {userName}. Could you rephrase? Try asking about passwords, phishing, scams, or safe browsing.";
       }
       // === Follow-up method for conversation flow ===
       public string GetFollowUp(string topic)
       {
           if (followUpResponses.ContainsKey(topic))
               return followUpResponses[topic];
           return "I do not have more details on that topic right now. Try asking about passwords, phishing, or malware!";
       }
   }
}