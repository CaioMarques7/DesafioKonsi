using ExtratoClube.DTO.Resposta;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ExtratoClube.Servicos
{
    /// <summary>
    /// Define operações para extração de benefícios
    /// </summary>
    public interface IServicoDeExtracao : IDisposable
    {
        /// <summary>
        /// Preenche a credencial de login
        /// </summary>
        /// <param name="login">Login para extração de benefício</param>
        /// <returns>Instância do serviço atualizada</returns>
        IServicoDeExtracao PreencherLogin(string login);

        /// <summary>
        /// Preenche a credencial de senha
        /// </summary>
        /// <param name="senha">Senha para extração de benefício</param>
        /// <returns>Instância do serviço atualizada</returns>
        IServicoDeExtracao PreencherSenha(string senha);

        /// <summary>
        /// Realiza a consulta dos benefícios de um CPF
        /// </summary>
        /// <param name="numeroDoCpf">Número do CPF a ser consultado</param>
        /// <param name="cancellationToken">Token para cancelamento de execução</param>
        /// <returns>Dados da consulta</returns>
        Task<RespostaDeConsulta> ConsultarBeneficios(string numeroDoCpf, CancellationToken cancellationToken);
    }
}
