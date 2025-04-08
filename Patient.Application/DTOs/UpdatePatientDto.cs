using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patient.Application.DTOs
{
    public class UpdatePatientDto
    {
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
        /// Informações de contato (telefone, e-mail, etc.).
        /// </summary>
        public string Contact { get; set; }
    }
}