using Patient.Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Patient.Application.Interfaces
{
    public interface IPatientService
    {
        /// <summary>
        /// Obtém todos os pacientes cadastrados.
        /// </summary>
        Task<IEnumerable<PatientDto>> GetAllPatientsAsync();

        /// <summary>
        /// Obtém um paciente específico pelo ID.
        /// </summary>
        Task<PatientDto> GetPatientByIdAsync(int id);

        /// <summary>
        /// Cria um novo paciente.
        /// </summary>
        Task<PatientDto> CreatePatientAsync(CreatePatientDto patientDto);

        /// <summary>
        /// Atualiza os dados de um paciente.
        /// </summary>
        Task<PatientDto> UpdatePatientAsync(int id, UpdatePatientDto patientDto);

        /// <summary>
        /// Remove um paciente (exclusão permanente ou soft delete).
        /// </summary>
        Task<bool> DeletePatientAsync(int id);
    }
}