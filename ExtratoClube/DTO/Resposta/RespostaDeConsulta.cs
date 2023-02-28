using Newtonsoft.Json;
using System.Collections.Generic;

namespace ExtratoClube.DTO.Resposta
{
    /// <summary>
    /// Modelo de dados que representa uma consulta de benefícios
    /// </summary>
    public sealed class RespostaDeConsulta
    {
        /// <summary>
        /// Inicia uma nova instância de <see cref="RespostaDeConsulta"/>
        /// </summary>
        /// <param name="cpf">Número do CPF consultado</param>
        /// <param name="beneficios">Lista de benefícios associados ao CPF</param>
        [JsonConstructor]
        public RespostaDeConsulta(string cpf, IEnumerable<RespostaDeBeneficio> beneficios)
        {
            NumeroDoCpf = cpf;
            Beneficios = beneficios;
        }

        /// <summary>
        /// Número do CPF consultado
        /// </summary>
        public string NumeroDoCpf { get; }

        /// <summary>
        /// Lista de benefícios associados ao CPF
        /// </summary>
        public IEnumerable<RespostaDeBeneficio> Beneficios { get; }
    }
}
