namespace MaximTechnologyTasks.Models
{
    public class ServerResponseModel
    {
        public string ProcessedString { get; set; } = null!;
        public Dictionary<char, int> CharsContent { get; set; } = null!;
        public string? LongestSubstring { get; set; }
        public string? SortedInvertedString { get; set; }
        public string ProcessedStringWithoutChar { get; set; } = null!;
    }
}
