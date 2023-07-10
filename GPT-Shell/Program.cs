// See https://aka.ms/new-console-template for more information

using OpenAI;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using static System.Net.Mime.MediaTypeNames;
using System.Diagnostics;
using System.Speech.Synthesis;
using System.Runtime.InteropServices;

var config = new ConfigurationBuilder()
    .AddUserSecrets(Assembly.GetExecutingAssembly())
    .Build();

string platform = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "Windows" :
                       RuntimeInformation.IsOSPlatform(OSPlatform.OSX) ? "MacOS" : "Linux";



var gpt = new GPT(config["API-Key"]);

var user = Environment.GetEnvironmentVariable("USER");

while (true)
{
    Console.WriteLine(platform);
    Console.Write($"{user}> ");
    var responseText = await gpt.completionRequest($"genera {platform} script, {Console.ReadLine()}");

    string script = Path.GetTempFileName();
    File.WriteAllText(script, responseText);


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

