using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Patient.Application.DTOs;
using Patient.Application.Interfaces;
using Patient.Domain.Entities;
using Patient.Infrastructure.Data;

namespace Patient.Infrastructure.Services
{
    public class PatientService : IPatientService
    {
        private readonly ApplicationDbContext _context;

        public PatientService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PatientDto>> GetAllPatientsAsync()
        {
            var patients = await _context.Patients.ToListAsync();
            return patients.Select(p => new PatientDto
            {
                Id = p.Id,
                Name = p.Name,
                CPF = p.CPF,
                DateOfBirth = p.DateOfBirth,
                Contact = p.Contact
            });
        }

        public async Task<PatientDto> GetPatientByIdAsync(int id)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient == null)
                return null;

            return new PatientDto
            {
                Id = patient.Id,
                Name = patient.Name,
                CPF = patient.CPF,
                DateOfBirth = patient.DateOfBirth,
                Contact = patient.Contact
            };
        }

        public async Task<PatientDto> CreatePatientAsync(CreatePatientDto patientDto)
        {
            var patient = new Domain.Entities.Patient
            {
                Name = patientDto.Name,
                CPF = patientDto.CPF,
                DateOfBirth = patientDto.DateOfBirth,
                Contact = patientDto.Contact
            };

            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();

            return new PatientDto
            {
                Id = patient.Id,
                Name = patient.Name,
                CPF = patient.CPF,
                DateOfBirth = patient.DateOfBirth,
                Contact = patient.Contact
            };
        }

        public async Task<PatientDto> UpdatePatientAsync(int id, UpdatePatientDto patientDto)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient == null)
                return null;

            patient.Name = patientDto.Name;
            patient.CPF = patientDto.CPF;
            patient.DateOfBirth = patientDto.DateOfBirth;
            patient.Contact = patientDto.Contact;

            _context.Patients.Update(patient);
            await _context.SaveChangesAsync();

            return new PatientDto
            {
                Id = patient.Id,
                Name = patient.Name,
                CPF = patient.CPF,
                DateOfBirth = patient.DateOfBirth,
                Contact = patient.Contact
            };
        }

        public async Task<bool> DeletePatientAsync(int id)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient == null)
                return false;

            _context.Patients.Remove(patient);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}