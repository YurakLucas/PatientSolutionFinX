namespace Patient.Application.DTOs
{
    public class PatientDto
    {
        /// <summary>
        /// Identificador do paciente (gerado automaticamente pelo banco de dados).
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
        /// Informações de contato (telefone, e-mail, etc.).
        /// </summary>
        public string Contact { get; set; }
    }
}