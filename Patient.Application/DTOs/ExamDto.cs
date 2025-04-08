namespace Patient.Application.DTOs
{
    public class ExamDto
    {
        /// <summary>
        /// Nome do exame.
        /// </summary>
        public string ExamName { get; set; }

        /// <summary>
        /// Resultado do exame.
        /// </summary>
        public string Result { get; set; }

        /// <summary>
        /// Data em que o exame foi realizado.
        /// </summary>
        public DateTime ExamDate { get; set; }
    }
}