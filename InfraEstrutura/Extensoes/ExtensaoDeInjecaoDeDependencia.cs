using ExtratoClube.Extensoes;
using InfraEstrutura.Servicos;
using InfraEstrutura.Servicos.Classes;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace InfraEstrutura.Extensoes
{
    /// <summary>
    /// Classe para extensões de injeção de dependência
    /// </summary>
    public static class ExtensaoDeInjecaoDeDependencia
    {
        /// <summary>
        /// Registra as dependências de infra-estrutura
        /// </summary>
        /// <param name="servicos">Instância do container de injeção de dependência</param>
        /// <param name="ambienteDeTestes">Indicador de execução em ambiente de testes</param>
        /// <returns>Instância do container de injeção de dependência</returns>
        public static IServiceCollection RegistrarDependenciasDeInfraEstrutura(this IServiceCollection servicos, bool ambienteDeTestes)
            => servicos.AddSingleton<IAdaptadorDeConsultaDeBeneficios, AdaptadorDeConsultaDeBeneficios>()
                .InjetarCondicionalmente(() => !ambienteDeTestes, servico => servico.RegistrarDependenciasDoExtratoClube());

        private static IServiceCollection InjetarCondicionalmente(this IServiceCollection servicos, Func<bool> condicao, Func<IServiceCollection, IServiceCollection> expressao)
            => condicao?.Invoke() ?? false ? expressao(servicos) : servicos;
    }
}