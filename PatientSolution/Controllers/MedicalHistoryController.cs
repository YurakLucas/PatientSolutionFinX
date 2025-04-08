using Patient.Application.DTOs;
using Patient.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Patient.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class MedicalHistoryController : ControllerBase
    {
        private readonly IMedicalHistoryService _historyService;

        public MedicalHistoryController(IMedicalHistoryService historyService)
        {
            _historyService = historyService;
        }

        // GET: api/medicalhistory/{patientId}
        [HttpGet("{patientId}")]
        public async Task<IActionResult> GetHistoryByPatient(int patientId)
        {
            var histories = await _historyService.GetMedicalHistoryByPatientAsync(patientId);
            return Ok(histories);
        }

        // POST: api/medicalhistory
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateMedicalHistoryDto historyDto)
        {
            if (historyDto == null)
                return BadRequest("Payload inválido.");

            var createdHistory = await _historyService.CreateMedicalHistoryAsync(historyDto);
            return CreatedAtAction(nameof(GetHistoryByPatient), new { patientId = createdHistory.PatientId }, createdHistory);
        }

        // PUT: api/medicalhistory/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateMedicalHistoryDto historyDto)
        {
            if (historyDto == null)
                return BadRequest("Payload inválido.");

            var updatedHistory = await _historyService.UpdateMedicalHistoryAsync(id, historyDto);
            if (updatedHistory == null)
                return NotFound();

            return Ok(updatedHistory);
        }

        // DELETE: api/medicalhistory/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _historyService.DeleteMedicalHistoryAsync(id);
            if (!result)
                return NotFound();
            return NoContent();
        }
    }
}