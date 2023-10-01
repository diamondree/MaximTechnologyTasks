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

        public StringFormatController(StringFormatService formatService)
        {
            _formatService = formatService;
        }

        [HttpGet]
        public async Task<ObjectResult> FormatStr([FromQuery] InputModel model) {
            try
            { 
                var response = await _formatService.FormatStr(model);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            }
            
    }
}
