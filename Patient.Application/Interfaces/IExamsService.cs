using Patient.Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Patient.Application.Interfaces
{
    public interface IExamsService
    {
        /// <summary>
        /// Consulta exames externos a partir de uma API REST, permitindo filtrar por um parâmetro.
        /// </summary>
        Task<IEnumerable<ExamDto>> GetExamsAsync(string filter);
    }
}