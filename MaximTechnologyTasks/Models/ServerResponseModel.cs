using MaximTechnologyTasks.Interfaces;

namespace MaximTechnologyTasks.Models
{
    public class ServerResponseModel : IResultResponse
    {
        public string ProcessedString { get; set; } = null!;
        public Dictionary<char, int> CharsContent { get; set; } = null!;
        public string? LongestSubstring { get; set; }
    }
}
