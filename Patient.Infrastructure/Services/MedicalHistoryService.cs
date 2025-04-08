using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Patient.Application.DTOs;
using Patient.Application.Interfaces;
using Patient.Domain.Entities;
using Patient.Infrastructure.Data;

namespace Patient.Infrastructure.Services
{
    public class MedicalHistoryService : IMedicalHistoryService
    {
        private readonly ApplicationDbContext _context;

        public MedicalHistoryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MedicalHistoryDto>> GetMedicalHistoryByPatientAsync(int patientId)
        {
            var histories = await _context.MedicalHistories
                .Where(h => h.PatientId == patientId && !h.IsDeleted)
                .ToListAsync();

            return histories.Select(h => new MedicalHistoryDto
            {
                Id = h.Id,
                PatientId = h.PatientId,
                Diagnoses = h.Diagnoses,
                Exams = h.ExamsPerformed,
                Prescriptions = h.Prescriptions,
                RecordDate = h.RecordDate
            });
        }

        public async Task<MedicalHistoryDto> CreateMedicalHistoryAsync(CreateMedicalHistoryDto historyDto)
        {
            var history = new MedicalHistory
            {
                PatientId = historyDto.PatientId,
                Diagnoses = historyDto.Diagnoses,
                ExamsPerformed = historyDto.Exams,
                Prescriptions = historyDto.Prescriptions,
                RecordDate = historyDto.RecordDate,
                IsDeleted = false
            };

            _context.MedicalHistories.Add(history);
            await _context.SaveChangesAsync();

            return new MedicalHistoryDto
            {
                Id = history.Id,
                PatientId = history.PatientId,
                Diagnoses = history.Diagnoses,
                Exams = history.ExamsPerformed,
                Prescriptions = history.Prescriptions,
                RecordDate = history.RecordDate
            };
        }

        public async Task<MedicalHistoryDto> UpdateMedicalHistoryAsync(int id, UpdateMedicalHistoryDto historyDto)
        {
            var history = await _context.MedicalHistories.FindAsync(id);
            if (history == null)
                return null;

            history.PatientId = historyDto.PatientId;
            history.Diagnoses = historyDto.Diagnoses;
            history.ExamsPerformed = historyDto.Exams;
            history.Prescriptions = historyDto.Prescriptions;
            history.RecordDate = historyDto.RecordDate;

            _context.MedicalHistories.Update(history);
            await _context.SaveChangesAsync();

            return new MedicalHistoryDto
            {
                Id = history.Id,
                PatientId = history.PatientId,
                Diagnoses = history.Diagnoses,
                Exams = history.ExamsPerformed,
                Prescriptions = history.Prescriptions,
                RecordDate = history.RecordDate
            };
        }

        public async Task<bool> DeleteMedicalHistoryAsync(int id)
        {
            var history = await _context.MedicalHistories.FindAsync(id);
            if (history == null)
                return false;

            // Realiza o soft delete
            history.IsDeleted = true;
            _context.MedicalHistories.Update(history);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}