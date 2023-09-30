using MaximTechnologyTasks.Interfaces;

namespace MaximTechnologyTasks.Models
{
    public class ServerErrorModel : IResultResponse
    {
        public List<ErrorModel> Errors { get; set; } = null!;
    }
}
