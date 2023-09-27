using MaximTechnologyTasks.Services;
using Microsoft.AspNetCore.Mvc;

namespace MaximTechnologyTasks.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class StringFormatController : ControllerBase
    {
        private readonly StringFormatService _formatService;

        public StringFormatController (StringFormatService formatService)
        {
            _formatService = formatService;
        }

        [HttpGet]
        public async Task<string> FormatStr(string origin)
            => await _formatService.FormatStr(origin);
    }
}
