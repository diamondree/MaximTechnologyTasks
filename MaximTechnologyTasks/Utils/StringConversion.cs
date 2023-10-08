using MaximTechnologyTasks.Configs;
using Microsoft.Extensions.Options;
using System.Runtime;

namespace MaximTechnologyTasks.Utils
{
    public static class StringConversion
    {
        public static async Task<string> ReformString(string origin)
        {
            if (origin.Length % 2 == 0)

                return String.Concat(
                    await ReverseString(String.Concat(origin.Take(origin.Length / 2))),
                    await ReverseString(String.Concat(origin.TakeLast(origin.Length / 2)))
                    );
            else
                return String.Concat(await ReverseString(origin), origin);
        }

        
        public static async Task<Dictionary<char, int>> GetCharsCountInString(string reversedString)
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

        
        public static async Task<string> GetLongestSubstring(string reversedString)
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


        private static async Task<string> ReverseString(string origin)
        {
            char[] reversedString = new char[origin.Length];
            for (int i = origin.Length - 1; i >= 0; i--)
                reversedString[origin.Length - i - 1] = origin[i];
            return new string(reversedString);
        }
    }
}
