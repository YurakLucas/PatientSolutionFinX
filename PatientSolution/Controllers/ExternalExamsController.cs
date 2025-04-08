using Patient.Application.DTOs;
using Patient.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Patient.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class ExternalExamsController : ControllerBase
    {
        private readonly IExamsService _examsService;

        public ExternalExamsController(IExamsService examsService)
        {
            _examsService = examsService;
        }

        // GET: api/externalexams?filter={filter}
        [HttpGet]
        public async Task<IActionResult> GetExams([FromQuery] string filter)
        {
            var exams = await _examsService.GetExamsAsync(filter);
            return Ok(exams);
        }
    }
}
