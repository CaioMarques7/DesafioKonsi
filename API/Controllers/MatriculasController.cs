using API.DTO.Requisicao;
using API.DTO.Resposta;
using FluentValidation;
using FluentValidation.Results;
using InfraEstrutura.Servicos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace API.Controllers
{
    /// <summary>
    /// Serviços para consulta de benefícios
    /// </summary>
    [ApiController, Route("matriculas"), ApiExplorerSettings(GroupName = "Matrículas")]
    [Consumes("application/json"), Produces("application/json")]
    public class MatriculasController : ControllerBase
    {
        private readonly IValidator<SolicitacaoDeConsultaDeMatricula> _validador;
        private readonly IAdaptadorDeConsultaDeBeneficios _adaptador;

        /// <summary>
        /// Inicia uma nova instância de <see cref="MatriculasController"/>
        /// </summary>
        /// <param name="validador">Instância do validador da requisição</param>
        /// <param name="adaptador">Instância do adaptador de consulta de benefícios</param>
        public MatriculasController(IValidator<SolicitacaoDeConsultaDeMatricula> validador, IAdaptadorDeConsultaDeBeneficios adaptador)
        {
            _validador = validador;
            _adaptador = adaptador;
        }

        /// <summary>
        /// Consulta os benefícios vinculados a um CPF
        /// </summary>
        /// <remarks>
        /// Exemplo de requisição:
        ///
        ///     POST /matriculas
        ///     {
        ///         "login": "testekonsi",
        ///         "senha": "testekonsi",
        ///         "numeroDoCpf": "124.294.735-34"
        ///     }
        ///     
        /// </remarks>
        /// <param name="requisicao">Dados da solicitação</param>
        /// <param name="cancellationToken">Token para notificar o cancelamento da execução</param>
        /// <returns>Dados dos benefícios encontrados</returns>
        /// <response code="200">Dados dos benefícios encontrados</response>
        /// <response code="400">Requisição inválida</response>
        /// <response code="500">Erro interno do servidor</response>
        /// <response code="502">Erro na comunicação com serviços externos</response>
        [HttpPost]
        [ProducesResponseType(typeof(RespostaDeConsultaDeMatricula), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status502BadGateway)]
        public async Task<IActionResult> Consultar([FromBody] SolicitacaoDeConsultaDeMatricula requisicao, CancellationToken cancellationToken)
        {
            try
            {
                var resultadoDeValidacao = _validador.Validate(requisicao);

                if (!resultadoDeValidacao.IsValid)
                    return RetornarErros(StatusCodes.Status400BadRequest, resultadoDeValidacao);

                var (NumeroDoCpf, NumerosDeBeneficio) = await _adaptador.ConsultarBeneficios(requisicao.NumeroDoCpf, requisicao.Login, requisicao.Senha, cancellationToken).ConfigureAwait(false);

                return StatusCode(StatusCodes.Status200OK, new RespostaDeConsultaDeMatricula(NumeroDoCpf, NumerosDeBeneficio));
            }
            catch (HttpRequestException ex)
            {
                return RetornarErros(StatusCodes.Status502BadGateway, ex?.InnerException?.Message ?? ex.Message);
            }
            catch (Exception ex)
            {
                return RetornarErros(StatusCodes.Status500InternalServerError, ex?.InnerException?.Message ?? ex.Message);
            }
        }

        private static ObjectResult RetornarErros(int statusCode, string mensagem)
        {
            var retorno = new HttpValidationProblemDetails()
            {
                Title = "Erro ao consultar matrículas",
                Status = statusCode,
                Detail = mensagem
            };

            return new ObjectResult(retorno)
            {
                ContentTypes = { "application/problem+json" }
            };
        }

        private static ObjectResult RetornarErros(int statusCode, ValidationResult resultadoDeValidacao)
        {
            var retorno = new HttpValidationProblemDetails(resultadoDeValidacao.ToDictionary())
            {
                Title = "Erro ao consultar matrículas",
                Status = statusCode
            };

            return new ObjectResult(retorno)
            {
                ContentTypes = { "application/problem+json" }
            };
        }
    }
}