using MaximTechnologyTasks.Interfaces;
using MaximTechnologyTasks.Models;
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

        [HttpPost]
        public async Task<IResultResponse> FormatStr(InputModel model)
            => await _formatService.FormatStr(model);
    }
}
