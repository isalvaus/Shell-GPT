using System;
namespace OpenAI.Chat
{
	public class Request
	{
        public string model { get; set; } = "text-davinci-003";
        public Message[] messages { get; set; }
        public double? temperature { get; set; }
        public int? max_tokens { get; set; }
        
    }
}

