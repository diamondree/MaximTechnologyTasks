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


            if (origin.Length % 2 == 0)
            {
                serverResponse.Add(
                    String.Concat(
                    await ReverseString(String.Concat(origin.Take(origin.Length / 2))),
                    await ReverseString(String.Concat(origin.TakeLast(origin.Length / 2)))
                    ));
            }
            else
            {
                serverResponse.Add(String.Concat(await ReverseString(origin), origin));
            }

            

            foreach (var note in await GetCharsCountInString(serverResponse[0]))
            {
                serverResponse.Add($"Digit '{note.Key}' is contained in the processed string {note.Value} times");
            }

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

    }
}
