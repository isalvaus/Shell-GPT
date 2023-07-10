using System;
namespace OpenAI.Chat
{
    public class Message
    {
        /// <summary>
        /// The role of the author of this message. One of system, user, or assistant.
        /// </summary>
        public string role { get; set; } = "user";

        /// <summary>
        /// The content of the message
        /// </summary>
        public string content { get; set; }

        /// <summary>
        /// The name of the author of this message. 
        /// </summary>
        public string? name { get; set; }
    }
}

