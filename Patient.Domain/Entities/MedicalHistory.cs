using System;

namespace Patient.Domain.Entities
{
    public class MedicalHistory
    {
        /// <summary>
        /// Identificador único do registro de histórico médico.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Chave estrangeira para o paciente a quem este histórico pertence.
        /// </summary>
        public int PatientId { get; set; }

        /// <summary>
        /// Navegação para a entidade Patient.
        /// </summary>
        public Patient Patient { get; set; }

        /// <summary>
        /// Diagnósticos relacionados ao paciente.
        /// </summary>
        public string Diagnoses { get; set; }

        /// <summary>
        /// Exames realizados pelo paciente.
        /// </summary>
        public string? ExamsPerformed { get; set; }

        /// <summary>
        /// Prescrições médicas emitidas.
        /// </summary>
        public string Prescriptions { get; set; }

        /// <summary>
        /// Data em que o registro do histórico foi criado.
        /// </summary>
        public DateTime RecordDate { get; set; }

        /// <summary>
        /// Indicador de exclusão lógica (soft delete).
        /// </summary>
        public bool IsDeleted { get; set; }
    }
}