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

        
        public GPT()
        {
            var config = new ConfigurationBuilder()
                .AddUserSecrets<GPT>()
                .Build();
         

            http.BaseAddress = new Uri("https://api.openai.com");
            http.DefaultRequestHeaders.Authorization = new("Bearer", config["API-Key"]);
            http.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json")
            );
            
        }


        
        public string? prompt { get; set; }

        // Declare the parameters for the cmdlet.
        public string model { get; set; } = "text-davinci-003";

        public double? temperature { get; set; } = 0;
        public int? max_tokens { get; set; } = 2048;


        // Hace una peticion al endpoint Completions.
        public async Task<String?> completionRequest(String prompt, Double? temperature = null, int? max_tokens = null )
        {

            // Platform
            String platform = "";

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                platform = "Windows";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                platform = "Linux";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                platform = "MacOS";
            }


            var request = JsonSerializer.Serialize(
                 new Completions.Request {
                     model = this.model, 
                     prompt = $"genera {platform} script, {prompt}",
                     //messages = new[] { new Chat.Message() {content = $"genera {platform} script, {prompt}", name = Environment.GetEnvironmentVariable("USER")} },
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
                        var text = response.choices.First().text;
                        System.Diagnostics.Debug.WriteLine($"GPT response: {text}");

                        string script = Path.GetTempFileName();
                        File.WriteAllText(script, text);


                        Process process = new Process();

                        process.StartInfo.Verb = "runas";

                        process.StartInfo.FileName = "/bin/bash";
                        process.StartInfo.Arguments = script;

                        process.StartInfo.UseShellExecute = false;
                        process.StartInfo.RedirectStandardOutput = true;
                        process.StartInfo.CreateNoWindow = true;

                        process.Start();

                        Console.WriteLine(process.StandardOutput.ReadToEnd());

                    }
                }
            }
            catch (Exception ex){
                Console.WriteLine(ex);
            }

            

            return responseText;

        }

    }

}
