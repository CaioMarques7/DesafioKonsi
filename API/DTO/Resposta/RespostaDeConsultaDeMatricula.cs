using Newtonsoft.Json;
using System.Collections.Generic;

namespace API.DTO.Resposta
{
    /// <summary>
    /// Modelo de dados para resposta de consulta de matrícula
    /// </summary>
    public class RespostaDeConsultaDeMatricula
    {
        /// <summary>
        /// Inicia uma nova instância de <see cref="RespostaDeConsultaDeMatricula"/>
        /// </summary>
        /// <param name="numeroDoCpf">Número do CPF consultado</param>
        /// <param name="numerosDeBeneficio">Números de benfício associados ao CPF</param>
        public RespostaDeConsultaDeMatricula(string numeroDoCpf, IEnumerable<string> numerosDeBeneficio)
        {
            NumeroDoCpf = numeroDoCpf;
            NumerosDeBeneficio = numerosDeBeneficio;
        }

        /// <summary>
        /// Número do CPF consultado
        /// </summary>
        [JsonProperty("numeroDoCpf")]
        public string NumeroDoCpf { get; }

        /// <summary>
        /// Números de benfício associados ao CPF
        /// </summary>
        [JsonProperty("numerosDeBeneficio")]
        public IEnumerable<string> NumerosDeBeneficio { get; }
    }
}
