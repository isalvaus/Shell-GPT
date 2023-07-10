using System.Management.Automation;  // PowerShell namespace.
using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.PowerShell.Commands;
using Microsoft.Extensions.Configuration;
using System.Text;
using OpenAI;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace OpenAI
{
    // Declare the class as a cmdlet and specify and
    // appropriate verb and noun for the cmdlet name.

    public class GPT 
    {
        HttpClient http = new();

        public GPT(String APIKey)
        {

            http.BaseAddress = new Uri("https://api.openai.com");
            http.DefaultRequestHeaders.Authorization = new("Bearer", APIKey);
            http.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json")
            );
            
        }

        
        
        public string? prompt { get; set; }

        // Declare the parameters for the cmdlet.
        public string completions_model { get; set;} = "text-davinci-003";
        public string chat_model { get; set; } = "gpt-4";

        public double? temperature { get; set; } = 0;
        public int? max_tokens { get; set; } = 2048;


        // Hace una peticion al endpoint Completions.
        public async Task<String?> completionRequest(String prompt, Double? temperature = null, int? max_tokens = null )
        {

            var request = JsonSerializer.Serialize(
                 new Completions.Request {
                     model = this.completions_model, 
                     prompt = prompt,
                     temperature = temperature ?? this.temperature,
                     max_tokens = max_tokens ?? this.max_tokens
                 }); 

            String? responseText = null;

  
            System.Diagnostics.Debug.WriteLine(request);

            try
            {
                if (await http.PostAsync(Endpoints.Completions, new StringContent(request, Encoding.UTF8, "application/json")) is HttpResponseMessage httpResponse)
                {
                    if (await JsonSerializer.DeserializeAsync<Completions.Response>(httpResponse.Content.ReadAsStream()) is Completions.Response response)
                    {
                        responseText = response.choices.First().text;
                        System.Diagnostics.Debug.WriteLine($"GPT response: {responseText}");

                    }
                }
            }
            catch (Exception ex){
                Console.WriteLine(ex);
            }

            

            return responseText;

        }
        public async Task<String?> chatRequest(String content, Double? temperature = null, int? max_tokens = null)
        {

            var request = JsonSerializer.Serialize(
                 new Chat.Request
                 {
                     model = this.chat_model,
                     messages = new[] { new Chat.Message() {role = "user", content = content, name = Environment.GetEnvironmentVariable("USER")} },
                     temperature = temperature ?? this.temperature,
                     max_tokens = max_tokens ?? this.max_tokens
                 });

            String? responseText = null;


            System.Diagnostics.Debug.WriteLine(request);

            try
            {
                if (await http.PostAsync(Endpoints.Chat, new StringContent(request, Encoding.UTF8, "application/json")) is HttpResponseMessage httpResponse)
                {
                    if (await JsonSerializer.DeserializeAsync<Chat.Response>(httpResponse.Content.ReadAsStream()) is Chat.Response response)
                    {
                        responseText = response.choices.First().message.content;
                        System.Diagnostics.Debug.WriteLine($"GPT response: {responseText}");

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }



            return responseText;

        }

    }

}
