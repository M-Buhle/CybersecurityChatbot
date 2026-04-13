namespace CybersecurityChatbot.Responses
{
   public class ChatbotResponses
   {
       // This method takes what the user typed and returns the right answer
       public string GetResponse(string userInput)
       {
           // Convert to lowercase so it works no matter how they type
           string input = userInput.ToLower();
           if (string.IsNullOrWhiteSpace(input))
               return "It looks like you didn't type anything. Please ask me a cybersecurity question!";
           else if (input.Contains("password"))
               return "Always use strong passwords with at least 12 characters, mixing letters, numbers, and symbols. Never reuse the same password across different sites!";
           else if (input.Contains("phishing"))
               return "Phishing is when criminals send fake emails pretending to be trusted organisations to steal your info. Always verify the sender before clicking any link!";
           else if (input.Contains("browsing") || input.Contains("safe browse"))
               return "For safe browsing, always look for HTTPS in the website URL. Avoid clicking pop-up ads and never download files from untrusted sources.";
           else if (input.Contains("how are you"))
               return "I'm doing great and ready to help you stay safe online! What would you like to know?";
           else if (input.Contains("purpose") || input.Contains("what can you do"))
               return "I'm your Cybersecurity Awareness Assistant! I can help you learn about password safety, phishing scams, and safe browsing practices.";
           else if (input.Contains("what can i ask"))
               return "You can ask me about: passwords, phishing, safe browsing, or just say hello!";
           else
               return "I didn't quite understand that. Could you rephrase? Try asking about passwords, phishing, or safe browsing.";
       }
   }
}