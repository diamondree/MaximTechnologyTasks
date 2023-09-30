using System.Text;

namespace MaximTechnologyTasks.Services
{
    public class StringFormatService
    {
        public async Task<List<string>> FormatStr (string origin)
        {
            List<string> serverResponse = new List<string>();

            if (origin == null)
            {
                serverResponse.Add(string.Empty);
                return serverResponse;
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
                serverResponse.Add("Incorrect input letters: " + sb.ToString());
                return serverResponse;
            }


            // modify string with an even number of chars
            if (origin.Length % 2 == 0)
            {
                serverResponse.Add(
                    String.Concat(
                    await ReverseString(String.Concat(origin.Take(origin.Length / 2))),
                    await ReverseString(String.Concat(origin.TakeLast(origin.Length / 2)))
                    ));
            }
            // modify string with an odd number of chars
            else
            {
                serverResponse.Add(String.Concat(await ReverseString(origin), origin));
            }

            
            // adding chars count to server response
            foreach (var note in await GetCharsCountInString(serverResponse[0]))
            {
                serverResponse.Add($"Digit '{note.Key}' is contained in the processed string {note.Value} times");
            }


            // add longest substring starting and ending with any of "aeiouy"
            serverResponse.Add(await GetLongestSubstring(serverResponse[0]));


            return serverResponse;
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
                return ("String doesn`t contain substring");


            else if (symbolsCount == 1)
                return ($"The longest substring is: {reversedString.FirstOrDefault(c => symbols.Contains(c))}");
            

            int firstSymbolPos = reversedString.IndexOfAny(symbols);

            return (reversedString.Substring(firstSymbolPos, reversedString.LastIndexOfAny(symbols) - firstSymbolPos + 1));
        }

    }
}
