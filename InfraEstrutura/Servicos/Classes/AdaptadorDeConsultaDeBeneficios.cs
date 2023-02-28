using ExtratoClube.Servicos;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraEstrutura.Servicos.Classes
{
    internal sealed class AdaptadorDeConsultaDeBeneficios : IAdaptadorDeConsultaDeBeneficios
    {
        private readonly IServicoDeExtracao _servicoDeExtracao;

        public AdaptadorDeConsultaDeBeneficios(IServicoDeExtracao servicoDeExtracao)
        {
            _servicoDeExtracao = servicoDeExtracao;
        }

        /// <inheritdoc />
        public async Task<(string NumeroDoCpf, IEnumerable<string> NumerosDeBeneficio)> ConsultarBeneficios(string numeroDoCpf, string login, string senha, CancellationToken cancellationToken)
        {
            _ = _servicoDeExtracao.PreencherLogin(login)
                .PreencherSenha(senha);

            var consulta = await _servicoDeExtracao.ConsultarBeneficios(numeroDoCpf, cancellationToken).ConfigureAwait(false);

            return (consulta.NumeroDoCpf, consulta.Beneficios.Select(_ => _.NumeroDoBeneficio));
        }
    }
}
