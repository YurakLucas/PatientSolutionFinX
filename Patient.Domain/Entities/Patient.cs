using System;
using System.Collections.Generic;

namespace Patient.Domain.Entities
{
    public class Patient
    {
        /// <summary>
        /// Identificador único do paciente (gerado automaticamente).
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Nome completo do paciente.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// CPF do paciente.
        /// </summary>
        public string CPF { get; set; }

        /// <summary>
        /// Data de nascimento do paciente.
        /// </summary>
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// Informações de contato do paciente (telefone, email, etc.).
        /// </summary>
        public string Contact { get; set; }

        /// <summary>
        /// Lista de históricos médicos associados ao paciente.
        /// </summary>
        public ICollection<MedicalHistory> MedicalHistories { get; set; } = new List<MedicalHistory>();
    }
}
