using Patient.Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Patient.Application.Interfaces
{
    public interface IMedicalHistoryService
    {
        /// <summary>
        /// Obtém o histórico médico de um paciente.
        /// </summary>
        Task<IEnumerable<MedicalHistoryDto>> GetMedicalHistoryByPatientAsync(int patientId);

        /// <summary>
        /// Cria um novo registro no histórico médico.
        /// </summary>
        Task<MedicalHistoryDto> CreateMedicalHistoryAsync(CreateMedicalHistoryDto historyDto);

        /// <summary>
        /// Atualiza um registro no histórico médico.
        /// </summary>
        Task<MedicalHistoryDto> UpdateMedicalHistoryAsync(int id, UpdateMedicalHistoryDto historyDto);

        /// <summary>
        /// Remove um registro do histórico médico (soft delete ou deleção lógica).
        /// </summary>
        Task<bool> DeleteMedicalHistoryAsync(int id);
    }
}