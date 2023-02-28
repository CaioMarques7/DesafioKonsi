using Newtonsoft.Json;

namespace API.DTO.Requisicao
{
    /// <summary>
    /// Modelo de dados para consulta de matrícula
    /// </summary>
    public sealed class SolicitacaoDeConsultaDeMatricula
    {
        /// <summary>
        /// Inicia uma nova instância de <see cref="SolicitacaoDeConsultaDeMatricula"/>
        /// </summary>
        /// <param name="login">Login do serviço</param>
        /// <param name="senha">Senha do serviço</param>
        /// <param name="numeroDoCpf">Número do CPF a ser consultado</param>
        public SolicitacaoDeConsultaDeMatricula(string login, string senha, string numeroDoCpf)
        {
            Login = login;
            Senha = senha;
            NumeroDoCpf = numeroDoCpf;
        }

        /// <summary>
        /// Login do serviço
        /// </summary>
        [JsonProperty("login")]
        public string Login { get; private set; }

        /// <summary>
        /// Senha do serviço
        /// </summary>
        [JsonProperty("senha")]
        public string Senha { get; private set; }

        /// <summary>
        /// Número do CPF a ser consultado
        /// </summary>
        [JsonProperty("numeroDoCpf")]
        public string NumeroDoCpf { get; private set; }

        /// <inheritdoc />
        public override string ToString()
            => JsonConvert.SerializeObject(this);
    }
}
