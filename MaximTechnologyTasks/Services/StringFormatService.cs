using MaximTechnologyTasks.Interfaces;
using MaximTechnologyTasks.Models;
using System.Text;

namespace MaximTechnologyTasks.Services
{
    public class StringFormatService
    {
        public async Task<IResultResponse> FormatStr (InputModel model)
        {
            string origin = model.Origin;

            if (origin == null)
            {
                ServerErrorModel errorResponse = new ServerErrorModel();
                List<ErrorModel> errors = new List<ErrorModel>
                {
                    new ErrorModel
                    {
                        Code = 1,
                        Message = "Empty input"
                    }
                };
                errorResponse.Errors = errors;

                return errorResponse;
            }


            // checking for compliance with the condition
            if (! await IsSmallLetters(origin))
            {
                StringBuilder sb = new StringBuilder();

                foreach (char c in origin)
                {
                    if (!(c >= 'a' && c <= 'z'))
                        sb.Append(c);
                }

                //throw new Exception(message: "Incorrect input letters: " + sb.ToString());
                ServerErrorModel errorResponse = new ServerErrorModel();
                List<ErrorModel> errors = new List<ErrorModel>
                {
                    new ErrorModel
                    {
                        Code = 2,
                        Message = $"Incorrect input letters: '{sb}'"
                    }
                };
                errorResponse.Errors = errors;
                
                return errorResponse;
            }


            ServerResponseModel response = new ServerResponseModel();


            // modify string with an even number of chars
            if (origin.Length % 2 == 0)
            {
                response.ProcessedString = String.Concat(
                    await ReverseString(String.Concat(origin.Take(origin.Length / 2))),
                    await ReverseString(String.Concat(origin.TakeLast(origin.Length / 2)))
                    );
                    
            }
            // modify string with an odd number of chars
            else
            {
                response.ProcessedString = String.Concat(await ReverseString(origin), origin);
            }


            // adding chars count to server response            
            response.CharsContent = await GetCharsCountInString(response.ProcessedString);


            // add longest substring starting and ending with any of "aeiouy"
            response.LongestSubstring = await GetLongestSubstring(response.ProcessedString);


            // Sort string with required method
            if (model.FormatMethod.ToLower() == "quicksort")
                response.SortedInvertedString = new string(SortService.QuickSort(response.ProcessedString.ToCharArray()));

            if(model.FormatMethod.ToLower() == "treesort" || model.FormatMethod.ToLower() == "tree sort")
                response.SortedInvertedString = new string(SortService.TreeSort(response.ProcessedString.ToCharArray()));
            


            return response;
        }


        private async Task<bool> IsSmallLetters (string origin)
            => origin.All(x => (x >= 'a' && x <= 'z'));


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

    }
}
