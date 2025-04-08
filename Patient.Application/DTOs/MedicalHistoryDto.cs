namespace Patient.Application.DTOs
{
    public class MedicalHistoryDto
    {
        /// <summary>
        /// Identificador do histórico médico.
        /// </summary>
        public int Id { get; set; }

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