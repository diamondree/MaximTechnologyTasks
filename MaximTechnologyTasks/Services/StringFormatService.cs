using MaximTechnologyTasks.Configs;
using MaximTechnologyTasks.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Text;

namespace MaximTechnologyTasks.Services
{
    public class StringFormatService
    {
        private readonly IOptions<StringFormatServiceSettings> _settings;
        public StringFormatService(IOptions<StringFormatServiceSettings> settings) 
        {
            _settings = settings;
        }

        public async Task<ServerResponseModel> FormatStr (InputModel model)
        {
            string origin = model.Origin ?? throw new Exception(message: "Empty input");


            // checking for compliance with the condition
            if (! await IsSmallLetters(origin))
            {
                StringBuilder sb = new StringBuilder();

                foreach (char c in origin)
                {
                    if (!(c >= 'a' && c <= 'z'))
                        sb.Append(c);
                }

                throw new Exception(message: "Incorrect input letters: " + sb.ToString());
            }

            
            // check origin string similar to string in black list
            if (_settings.Value.BlackList != null)
                if (_settings.Value.BlackList.Contains(origin))
                    throw new Exception(message: "Input string is in black list");
                


            ServerResponseModel response = new ServerResponseModel();

            //reform string
            response.ProcessedString = await ReformString(origin);


            // adding chars count to server response            
            response.CharsContent = await GetCharsCountInString(response.ProcessedString);


            // add longest substring starting and ending with any of "aeiouy"
            response.LongestSubstring = await GetLongestSubstring(response.ProcessedString);


            // Sort string with required method
            if (model.SortMethod.ToLower() == "quicksort")
                response.SortedInvertedString = new string(SortService.QuickSort(response.ProcessedString.ToCharArray()));

            if(model.SortMethod.ToLower() == "treesort" || model.SortMethod.ToLower() == "tree sort")
                response.SortedInvertedString = new string(SortService.TreeSort(response.ProcessedString.ToCharArray()));


            // Delete char from a position recieved from remote API randon number generator
            response.ProcessedStringWithoutChar = response.ProcessedString.Remove(await GetRandomInt(response.ProcessedString.Length - 1), 1);

            return response;
        }


        private async Task<bool> IsSmallLetters (string origin)
            => origin.All(x => (x >= 'a' && x <= 'z'));


        private async Task<string> ReformString (string origin)
        {
            if (origin.Length % 2 == 0)
            
                return String.Concat(
                    await ReverseString(String.Concat(origin.Take(origin.Length / 2))),
                    await ReverseString(String.Concat(origin.TakeLast(origin.Length / 2)))
                    );
            else
                return String.Concat(await ReverseString(origin), origin);
        }


        private async Task<string> ReverseString (string origin)
        {
            char[] reversedString = new char[origin.Length];
            for (int i = origin.Length - 1; i >= 0; i--)
                reversedString[origin.Length - i - 1] = origin[i];
            return new string(reversedString);
        }


        private async Task<Dictionary<char, int>> GetCharsCountInString (string reversedString)
        {
            Dictionary<char, int> charsCount = new Dictionary<char, int>();

            foreach (char c in reversedString)
            {
                if (!charsCount.ContainsKey(c))
                {
                    charsCount.Add(c, 1);
                }
                else
                {
                    charsCount[c]++;
                }
            }

            return charsCount;
        }


        private async Task<string> GetLongestSubstring (string reversedString)
        {
            char[] symbols = { 'a', 'e', 'i', 'o', 'u', 'y' };

            var symbolsCount = reversedString.Count(x => symbols.Contains(x));


            if (symbolsCount == 0)
                return null;


            else if (symbolsCount == 1)
                return ($"{reversedString.FirstOrDefault(c => symbols.Contains(c))}");
            

            int firstSymbolPos = reversedString.IndexOfAny(symbols);

            return (reversedString.Substring(firstSymbolPos, reversedString.LastIndexOfAny(symbols) - firstSymbolPos + 1));
        }


        private async Task<int> GetRandomInt (int reversedStringLength)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri($"{_settings.Value.RandomApi}num=1&min=0&max={reversedStringLength}&col=1&base=10&format=plain&rnd=new");
                using (HttpResponseMessage response = await client.GetAsync(client.BaseAddress))
                {
                    try
                    {
                        response.EnsureSuccessStatusCode();
                        string responseBody = await response.Content.ReadAsStringAsync();
                        return Convert.ToInt32(responseBody);
                    }
                    catch (HttpRequestException ex)
                    {
                        Random rnd = new Random();
                        Console.WriteLine(ex.Message);
                        return rnd.Next(reversedStringLength);
                    }
                }
            }
        }

    }
}
