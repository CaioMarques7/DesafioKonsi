using Newtonsoft.Json;

namespace ExtratoClube.DTO.Resposta
{
    /// <summary>
    /// Modelo de dados que representa um benefício
    /// </summary>
    public sealed class RespostaDeBeneficio
    {
        /// <summary>
        /// Inicia uma nova instância de <see cref="RespostaDeBeneficio"/>
        /// </summary>
        /// <param name="id">ID do benefício</param>
        /// <param name="nb">Número do benefício</param>
        [JsonConstructor]
        public RespostaDeBeneficio(long id, string nb)
        {
            Id = id;
            NumeroDoBeneficio = nb;
        }

        /// <summary>
        /// ID do benefício
        /// </summary>
        public long Id { get; }

        /// <summary>
        /// Número do benefício
        /// </summary>
        public string NumeroDoBeneficio { get; }
    }
}
