using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;
using UnecontLogConverter.Services;
using UnecontLogConverter.ViewModels;

namespace UnecontLogConverter.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LogController : ControllerBase
    {
        [HttpPost("TransformLog")]
        public async Task<IActionResult> TransformLog([FromServices] LogService service, [FromQuery] string parameter, [FromQuery] bool saveToFile, [FromQuery] bool saveToDatabase)
        {
            if (string.IsNullOrEmpty(parameter))
                return BadRequest("Nenhum parâmetro foi fornecido.");

            var transformedLogs = await service.TransformLog(parameter, saveToFile, saveToDatabase);

            return Ok(transformedLogs);
        }
        [HttpGet("GetAllLogsAsync")]
        public async Task<IActionResult> GetAllLogsAsync([FromServices] LogService service, [FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate, [FromQuery] bool orderByDateAsc, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var logs = await service.GetAllLogsAsync(startDate, endDate, orderByDateAsc, pageNumber, pageSize);

            return Ok(logs);
        }

        [HttpGet("GetAllTransformedLogsAsync")]
        public async Task<IActionResult> GetAllTransformedLogsAsync([FromServices] LogService service, [FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate, [FromQuery] bool orderByDateAsc, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var logs = await service.GetAllTransformedLogsAsync(startDate, endDate, orderByDateAsc, pageNumber, pageSize);

            return Ok(logs);
        }

        [HttpGet("GetLogWithTransformation/{Id}")]
        public async Task<IActionResult> GetLogWithTransformation([FromServices] LogService service, string Id)
        {
            var logs = await service.GetLogWithTransformationAsync(Id);

            return Ok(logs);
        }

        [HttpGet("GetLog/{Id}")]
        public async Task<IActionResult> GetLog([FromServices] LogService service, string Id)
        {
            var logs = await service.GetLogAsync(Id);

            return Ok(logs);
        }

        [HttpGet("GetLogTransformation/{Id}")]
        public async Task<IActionResult> GetLogTransformation([FromServices] LogService service, string Id)
        {
            var logs = await service.GetLogTransformationAsync(Id);

            return Ok(logs);
        }
    }
}