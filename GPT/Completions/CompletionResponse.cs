using System;
using System.Text.Json.Serialization;

namespace OpenAI.Completions
{
	public class Response
    {
        public class Choice
        {
            public string text { get; set; }
            public int index { get; set; }
            public string? logprobs { get; set; }
            public string finish_reason { get; set; }
        }

        public class Usage {
            int prompt_tokens { get; set; }
            int completion_tokens { get; set; }
            int total_tokens { get; set; }
        }

        public string id { get; set; }

        [JsonPropertyName("object")]
        public string _object { get; set; }
        public int created { get; set; }
        public string model { get; set; }
        public Choice[] choices { get; set; }
        public Usage usage { get; set; }
        
		
	}
}

