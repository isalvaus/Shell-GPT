using System;
namespace OpenAI.Completions
{
	public class Request
	{
        public string model { get; set; } = "text-davinci-003";
        public string? prompt { get; set; }
        public double? temperature { get; set; }
        public int? max_tokens { get; set; }
        
    }
}

