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
            response.ProcessedString = await Utils.StringConversion.ReformString(origin);


            // adding chars count to server response            
            response.CharsContent = await Utils.StringConversion.GetCharsCountInString(response.ProcessedString);


            // add longest substring starting and ending with any of "aeiouy"
            response.LongestSubstring = await Utils.StringConversion.GetLongestSubstring(response.ProcessedString);


            // Sort string with required method
            if (model.SortMethod.ToLower() == "quicksort")
                response.SortedInvertedString = new string(SortService.QuickSort(response.ProcessedString.ToCharArray()));

            if(model.SortMethod.ToLower() == "treesort" || model.SortMethod.ToLower() == "tree sort")
                response.SortedInvertedString = new string(SortService.TreeSort(response.ProcessedString.ToCharArray()));


            // Delete char from a position recieved from remote API randon number generator
            response.ProcessedStringWithoutChar = await DelRandomCharFromString(response.ProcessedString);

            return response;
        }


        private async Task<bool> IsSmallLetters (string origin)
            => origin.All(x => (x >= 'a' && x <= 'z'));

        public async Task<string> DelRandomCharFromString(string origin)
            => origin.Remove(await GetRandomInt(origin.Length - 1), 1);


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
