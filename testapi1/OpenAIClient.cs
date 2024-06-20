using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace testapi1
{
    public class OpenAIClient
    {
        private readonly HttpClient _httpClient;

        public OpenAIClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetCompletionAsync(string prompt)
        {
            var request = new
            {
                model = "gpt-3.5-turbo",
                messages = new List<dynamic> { new
                {
                    role = "user",
                    content = prompt
                } },
                temperature = 0.7
            };

            var response = await _httpClient.PostAsJsonAsync("https://api.openai.com/v1/chat/completions", request);

            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadFromJsonAsync<OpenAIResponse>();

            return responseBody?.Choices[0].Message.Content.Trim() ?? string.Empty;
        }
    }

    public class OpenAIResponse
    {
        public string Id { get; set; }
        public string Object { get; set; }
        public long Created { get; set; }
        public string Model { get; set; }
        public Choice[] Choices { get; set; }
        public Usage Usage { get; set; }
    }

    public class Choice
    {
        public Message Message { get; set; }
        public string Logprobs { get; set; }
        public string Finish_reason { get; set; }
        public int Index { get; set; }
    }

    public class Message
    {
        public string Role { get; set; }
        public string Content { get; set; }
    }

    public class Usage
    {
        public int Prompt_tokens { get; set; }
        public int Completion_tokens { get; set; }
        public int Total_tokens { get; set; }
    }
}
