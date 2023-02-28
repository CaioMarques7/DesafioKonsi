using API;
using ExtratoClube.DTO.Resposta;
using ExtratoClube.Servicos;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading;

namespace Testes
{
    internal class TesteBase : IDisposable
    {
        private bool _recursoLiberado;

        public TesteBase()
        {

            Server = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.UseEnvironment(Program.NOME_AMBIENTE_TESTES);

                    builder.ConfigureAppConfiguration((context, builder) =>
                    {
                        builder.AddEnvironmentVariables();
                    });

                    builder.ConfigureServices((context, services) =>
                    {
                        services.AddSingleton(servico =>
                        {
                            var mockDeServicoDeExtracao = new Mock<IServicoDeExtracao>();

                            mockDeServicoDeExtracao.Setup(m => m.PreencherLogin(It.IsAny<string>()))
                                .Returns(mockDeServicoDeExtracao.Object);

                            mockDeServicoDeExtracao.Setup(m => m.PreencherSenha(It.IsAny<string>()))
                                .Returns(mockDeServicoDeExtracao.Object);

                            mockDeServicoDeExtracao.Setup(m => m.ConsultarBeneficios("15883990525", It.IsAny<CancellationToken>()))
                                .ReturnsAsync(JsonConvert.DeserializeObject<RespostaDeConsulta>("{\"id\":0,\"nome\":\"\",\"cpf\":\"15883990525\",\"sexo\":null,\"rg\":null,\"orgaoEmissor\":null,\"expedicao\":null,\"naturalidade\":null,\"ufNascimento\":null,\"ufDocumento\":null,\"nacionalidade\":null,\"escolaridade\":null,\"estadoCivil\":null,\"idade\":0,\"nasc\":\"\",\"status\":0,\"telefone1\":null,\"telefone2\":null,\"telefoneComercial\":null,\"celularLead\":null,\"celular1\":null,\"celular2\":null,\"parentesco\":null,\"pai\":null,\"mae\":null,\"email\":null,\"cnh\":null,\"preferencial\":null,\"identidade\":null,\"error\":null,\"beneficios\":[{\"id\":0,\"cbcPagadora\":0,\"agenciaIFPagadora\":0,\"status\":0,\"especie\":0,\"vlMargemEmprestimo\":0,\"vlMargemCartao\":0,\"vlBaseMargem\":0,\"salario\":0,\"margem\":0,\"nb\":\"1667726371\",\"descricaoSituacaoBeneficio\":null,\"pensaoAlimenticia\":null,\"representanteLegal\":null,\"bloqueadoParaEmprestimo\":null,\"beneficioElegivelParaEmprestimo\":null,\"nomeIFPagadora\":null,\"tipoCredito\":null,\"descricaoEspecieBeneficio\":null,\"cc\":null,\"cpfRepresentante\":null,\"nomeRepresentante\":null,\"senhaInss\":null,\"ultimaAtt\":null,\"dib\":null,\"contratos\":null,\"contratoDeCartao\":null,\"contratosDeCartao\":null,\"contratosDeCartaoRCC\":null,\"tipo\":null,\"competencia\":null,\"convenio\":null,\"mat_inst\":null,\"cod_orgao\":null,\"orgao\":null,\"campo11\":null,\"bruta_5\":0,\"util_5\":0,\"saldo_5\":0,\"bruta_30\":0,\"util_30\":0,\"saldo_30\":0,\"bruta_70\":0,\"util_70\":0,\"saldo_70\":0,\"rend_bruta\":0,\"desconto\":0,\"total_liquido\":0,\"excluido_parc\":0,\"excluido_valor\":0,\"sit_funcional\":null,\"margem_saldo\":0,\"nat_pensao\":null,\"cad_banco\":null,\"cad_agencia\":null,\"cad_conta\":null,\"regime_juridico\":null,\"cpf\":null,\"ag\":null,\"nit\":null,\"onlineStatus\":null,\"onlineData\":null,\"erroConsulta\":null,\"pessoa\":null,\"pessoaOffline\":null,\"datade\":null,\"limiar\":null,\"uf\":null,\"representacao\":null,\"procurador\":null,\"emprestimos\":0,\"despacho\":null,\"cartaobeneficio\":0,\"limite\":0}],\"enderecos\":null}"));

                            mockDeServicoDeExtracao.Setup(m => m.ConsultarBeneficios("01234567890", It.IsAny<CancellationToken>()))
                                .ThrowsAsync(new HttpRequestException());

                            mockDeServicoDeExtracao.Setup(m => m.ConsultarBeneficios("67353666030", It.IsAny<CancellationToken>()))
                                .ThrowsAsync(new Exception());

                            return mockDeServicoDeExtracao.Object;
                        });
                    });
                });
        }

        protected WebApplicationFactory<Program> Server { get; }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool liberarRecursosGerenciados)
        {
            if (!_recursoLiberado)
            {
                _recursoLiberado = true;

                if (liberarRecursosGerenciados)
                {
                    Server.Dispose();
                }
            }
        }
    }
}