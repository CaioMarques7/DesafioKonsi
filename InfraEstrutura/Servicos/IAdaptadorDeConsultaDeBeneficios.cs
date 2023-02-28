using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace InfraEstrutura.Servicos
{
    /// <summary>
    /// Define operações para consulta de matrículas através de uma camada Anti-Corrupção
    /// </summary>
    public interface IAdaptadorDeConsultaDeBeneficios
    {
        /// <summary>
        /// Consulta os benfícios do CPF informado
        /// </summary>
        /// <param name="numeroDoCpf">CPF que será consultado</param>
        /// <param name="login">Login para acesso ao serviço de consultas</param>
        /// <param name="senha">Senha para acesso ao serviço de consultas</param>
        /// <param name="cancellationToken">Token para cancelamento de execução</param>
        /// <returns>Dados da consulta</returns>
        Task<(string NumeroDoCpf, IEnumerable<string> NumerosDeBeneficio)> ConsultarBeneficios(string numeroDoCpf, string login, string senha, CancellationToken cancellationToken);
    }
}
