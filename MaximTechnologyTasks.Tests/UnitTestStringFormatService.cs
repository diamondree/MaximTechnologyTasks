using MaximTechnologyTasks.Configs;
using MaximTechnologyTasks.Services;
using MaximTechnologyTasks.Utils;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace MaximTechnologyTasks.Tests
{
    [TestFixture]
    public class Tests
    {
        private StringFormatService _stringFormatService;
        [SetUp]
        public void Setup()
        {
            StringFormatServiceSettings settings = new()
            {
                BlackList = new List<string>{
                    "abc",
                    "cba",
                },
                RandomApi = "https://www.random.org/integers/?"
            };
            IOptions<StringFormatServiceSettings> options = Options.Create(settings);
            _stringFormatService = new StringFormatService(options);
        }


        //Test cases for task 1 and odd length of input string
        [TestCase("", "")]
        [TestCase("a", "aa")]
        [TestCase("abc", "cbaabc")]
        [TestCase("ffjll", "lljffffjll")]
        public async Task WhenInputOddLengthString_ThenReturnConcutReturnedAndOriginString(string origin, string reversed)
        {
            Assert.That(await StringConversion.ReformString(origin), Is.EqualTo(reversed));
        }


        //Test cases for task 1 and even length of input string
        [TestCase("", "")]
        [TestCase("string", "rtsgni")]
        [TestCase("people", "oepelp")]
        [TestCase("abcdef", "cbafed")]
        public async Task WhenInputEvenLengthString_ThenReturnHalfByHalfReversed(string origin, string reversed)
        {
            Assert.That(await StringConversion.ReformString(origin), Is.EqualTo(reversed));
        }


        //Test cases for task 2, validating input string
        [TestCase("123")]
        [TestCase("abC")]
        [TestCase("ab5cdsfkF")]
        [TestCase("DFGHJNBVF")]
        public async Task WhenInputStringIncorrect_ThenReturnException(string origin)
        {
            Assert.That(async () => await _stringFormatService.VerifyInputString(origin), Throws.Exception);
        }


        //Test cases for task 3, count chars in string
        [TestCase("abc")]
        [TestCase("abbc")]
        [TestCase("bbbbbbb")]
        [TestCase("abcabca")]
        public async Task WhenInputCorrectString_ThenCountChars(string origin)
        {
            var dict = new Dictionary<char, int>();
            switch (origin)
            {
                case "abbc":
                    dict = new Dictionary<char, int>() { { 'a', 1 },{ 'b', 2 },{ 'c', 1 } };
                    break;

                case "abc":
                    dict = new Dictionary<char, int>() { { 'a', 1 }, { 'b', 1 }, { 'c', 1 } };
                    break;

                case "bbbbbbb":
                    dict = new Dictionary<char, int>() { { 'b', 7 }};
                    break;

                case "abcabca":
                    dict = new Dictionary<char, int>() { { 'a', 3 }, { 'b', 2 }, { 'c', 2 } };
                    break;

                default:
                    break;
            }
            CollectionAssert.AreEqual(dict, await StringConversion.GetCharsCountInString(origin));
        }


        //Test cases for task 4, longest substring
        [TestCase("abca", "abca")]
        [TestCase("bna", "a")]
        [TestCase("fffff", null)]
        [TestCase("wertyjnbvuft", "ertyjnbvu")]
        public async Task WhenCorrectInput_ThenGetLongestSubstring(string origin, string substring)
        {
            Assert.That(await StringConversion.GetLongestSubstring(origin), Is.EqualTo(substring));
        }


        //Test cases for task 5, treesort
        [TestCase("cba","abc")]
        [TestCase("fkgms","fgkms")]
        [TestCase("maxim","aimmx")]
        [TestCase("afkkju","afjkku")]
        public async Task WhenCorrectInput_ThenTreeSortString(string origin, string sortedOrigin)
        {
            Assert.That(SortService.TreeSort(origin.ToCharArray()), Is.EqualTo(sortedOrigin));
        }

        //Test cases for task 5, quicksort
        [TestCase("cba", "abc")]
        [TestCase("fkgms", "fgkms")]
        [TestCase("maxim", "aimmx")]
        [TestCase("afkkju", "afjkku")]
        public async Task WhenCorrectInput_ThenQuickSortString(string origin, string sortedOrigin)
        {
            Assert.That(SortService.QuickSort(origin.ToCharArray()), Is.EqualTo(sortedOrigin));
        }

        
    }
}