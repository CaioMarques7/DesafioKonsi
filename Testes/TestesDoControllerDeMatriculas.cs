using API.DTO.Requisicao;
using FluentAssertions;
using NUnit.Framework;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Testes
{
    [TestFixture]
    internal sealed class TestesDoControllerDeMatriculas : TesteBase
    {
        [Test]
        [TestCase("15883990525", "testekonsi", "testekonsi")]
        public async Task DeveConsultarComSucesso(string numeroDoCpf, string login, string senha)
        {
            var dados = new SolicitacaoDeConsultaDeMatricula(login, senha, numeroDoCpf);

            using var httpClient = Server.CreateClient();
            using var requisicao = new HttpRequestMessage(HttpMethod.Post, "/matriculas");
            requisicao.Content = new StringContent(dados.ToString(), Encoding.UTF8, MediaTypeNames.Application.Json);

            var response = await httpClient.SendAsync(requisicao, CancellationToken.None);
            response.EnsureSuccessStatusCode();

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        [TestCase(null, "testekonsi", "testekonsi")]
        [TestCase("15883990525", null, "testekonsi")]
        [TestCase("15883990525", "testekonsi", null)]
        public async Task DeveRetornarBadRequestQuandoUmParametroNaoForInformado(string numeroDoCpf, string login, string senha)
        {
            var dados = new SolicitacaoDeConsultaDeMatricula(login, senha, numeroDoCpf);

            using var httpClient = Server.CreateClient();
            using var requisicao = new HttpRequestMessage(HttpMethod.Post, "/matriculas");
            requisicao.Content = new StringContent(dados.ToString(), Encoding.UTF8, MediaTypeNames.Application.Json);

            var response = await httpClient.SendAsync(requisicao, CancellationToken.None);
            
            response.IsSuccessStatusCode.Should().BeFalse();
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Test]
        [TestCase("01234567890", "testekonsi", "testekonsi")]
        public async Task DeveRetornarBadGatewayQuandoHouverFalhaNaChamadaHttpInterna(string numeroDoCpf, string login, string senha)
        {
            var dados = new SolicitacaoDeConsultaDeMatricula(login, senha, numeroDoCpf);

            using var httpClient = Server.CreateClient();
            using var requisicao = new HttpRequestMessage(HttpMethod.Post, "/matriculas");
            requisicao.Content = new StringContent(dados.ToString(), Encoding.UTF8, MediaTypeNames.Application.Json);

            var response = await httpClient.SendAsync(requisicao, CancellationToken.None);

            response.IsSuccessStatusCode.Should().BeFalse();
            response.StatusCode.Should().Be(HttpStatusCode.BadGateway);
        }

        [Test]
        [TestCase("67353666030", "testekonsi", "testekonsi")]
        public async Task DeveRetornarInternalServerErrorQuandoHouverErroDesconhecido(string numeroDoCpf, string login, string senha)
        {
            var dados = new SolicitacaoDeConsultaDeMatricula(login, senha, numeroDoCpf);

            using var httpClient = Server.CreateClient();
            using var requisicao = new HttpRequestMessage(HttpMethod.Post, "/matriculas");
            requisicao.Content = new StringContent(dados.ToString(), Encoding.UTF8, MediaTypeNames.Application.Json);

            var response = await httpClient.SendAsync(requisicao, CancellationToken.None);

            response.IsSuccessStatusCode.Should().BeFalse();
            response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        }
    }
}
