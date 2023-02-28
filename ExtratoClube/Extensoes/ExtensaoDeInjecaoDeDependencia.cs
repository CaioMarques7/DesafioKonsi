using ExtratoClube.Servicos;
using ExtratoClube.Servicos.Classes;
using Microsoft.Extensions.DependencyInjection;

namespace ExtratoClube.Extensoes
{
    /// <summary>
    /// Classe para definição de extensões de injeção de dependência
    /// </summary>
    public static class ExtensaoDeInjecaoDeDependencia
    {
        /// <summary>
        /// Registra as dependências dos serviços Extrato Clube
        /// </summary>
        /// <param name="servicos">Instância do container de injeção de dependência</param>
        /// <returns>Instância do container de injeção de dependência</returns>
        public static IServiceCollection RegistrarDependenciasDoExtratoClube(this IServiceCollection servicos)
            => servicos.AddSingleton<IServicoDeExtracao, ServicoDeExtracao>()
                .AddHttpClient();
    }
}
