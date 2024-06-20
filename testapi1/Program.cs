// See https://aka.ms/new-console-template for more information
using testapi1;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.Extensions.DependencyInjection;

Console.WriteLine("Hello, World!");

var serviceProvider = new ServiceCollection()
    .AddHttpClient<OpenAIClient>(client =>
    {
        var apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
        if (string.IsNullOrEmpty(apiKey))
        {
            throw new InvalidOperationException("Please set the OPENAI_API_KEY environment variable.");
        }

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
    })
    .Services
    .BuildServiceProvider();

var openAiClient = serviceProvider.GetRequiredService<OpenAIClient>();

Console.WriteLine("Enter your prompt:");
var prompt = Console.ReadLine();

var response = await openAiClient.GetCompletionAsync(prompt);

Console.WriteLine("Response from OpenAI:");
Console.WriteLine(response);
