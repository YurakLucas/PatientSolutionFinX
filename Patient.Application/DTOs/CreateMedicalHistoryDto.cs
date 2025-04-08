using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patient.Application.DTOs
{
    public class CreateMedicalHistoryDto
    {
        /// <summary>
        /// Identificador do paciente ao qual este histórico pertence.
        /// </summary>
        public int PatientId { get; set; }

        /// <summary>
        /// Diagnósticos registrados.
        /// </summary>
        public string Diagnoses { get; set; }

        /// <summary>
        /// Exames realizados.
        /// </summary>
        public string Exams { get; set; }

        /// <summary>
        /// Prescrições médicas.
        /// </summary>
        public string Prescriptions { get; set; }

        /// <summary>
        /// Data do registro do histórico.
        /// </summary>
        public DateTime RecordDate { get; set; }
    }
}
