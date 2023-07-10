using System.Management.Automation;  // PowerShell namespace.
using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.PowerShell.Commands;
//using Microsoft.Extensions.Configuration;
using System.Text;
using OpenAI;

namespace OpenAI;


// Declare the class as a cmdlet and specify and
// appropriate verb and noun for the cmdlet name.
[Cmdlet("GPT", "4")]
public class GPTCmd : Cmdlet
{
    HttpClient http = new();


    public GPTCmd()
    {
     /*   var config = new ConfigurationBuilder()
            .AddUserSecrets<GPT>()
            .Build();
     */

        http.BaseAddress = new Uri("https://dle.rae.es/data/");
        http.DefaultRequestHeaders.Authorization = new("Bearer", "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");
        http.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json")
        );
        
    }


    
    [Parameter(Mandatory = true,Position = 0)]
    public string prompt { get; set; }

    // Declare the parameters for the cmdlet.
    public string model { get; set; } = "text-davinci-003";

    public double? temperature { get; set; }
    public int? max_tokens { get; set; }
    public double? top_p { get; set; }

    // Override the ProcessRecord method to process
    // the supplied user name and write out a
    // greeting to the user by calling the WriteObject
    // method.
    protected override void ProcessRecord()
    {

        var request = JsonSerializer.Serialize(
             new Completions.Request {
                 model = this.model,
                 prompt = "PowerShell script for, " + this.prompt,
                 temperature = 0.3,
                 max_tokens = 100
             });
                




        System.Diagnostics.Debug.WriteLine(request);


        var response = http.PostAsync(Completions, new StringContent(request, Encoding.UTF8, "application/json")).GetAwaiter().GetResult();
        

        try
        {

            var completionResponse = JsonSerializer.Deserialize<CompletionResponse>(response.Content.ReadAsStream());

            if (completionResponse != null)
            {
                // Initialize PowerShell engine
                using (PowerShell powerShell = PowerShell.Create())
                {
                    var responseText = completionResponse.choices.First().text;
                    WriteObject(responseText);

                    // Add the script to the PowerShell object
                    powerShell.AddScript(" $interfaces = Get - NetAdapter\n  Write - Host \"hola\" ");

                    // Execute the script
                    var results = powerShell.Invoke();

                    WriteObject(results.Count);

                    // Display the results
                    foreach (PSObject result in results)
                    {
                        Console.WriteLine(result.ToString());
                    }
                }
            }
                
        }
        catch (Exception ex)
        {
            WriteObject(ex);
        }


    }

}
