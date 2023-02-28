using API.DTO.Requisicao;
using FluentValidation;

namespace API.Validadores
{
    /// <summary>
    /// Classe de regras de entrada do endpoint de consulta de matrícula
    /// </summary>
    internal sealed class ValidadorDeSolicitacaoDeConsultaDeMatricula : AbstractValidator<SolicitacaoDeConsultaDeMatricula>
    {
        /// <summary>
        /// Inicia uma nova instância de <see cref="ValidadorDeSolicitacaoDeConsultaDeMatricula"/>
        /// </summary>
        public ValidadorDeSolicitacaoDeConsultaDeMatricula()
        {
            RuleFor(_ => _.Login).NotEmpty().WithErrorCode("1").WithMessage("Login deve ser informado");
            RuleFor(_ => _.Senha).NotEmpty().WithErrorCode("2").WithMessage("Senha deve ser informada");
            RuleFor(_ => _.NumeroDoCpf).NotEmpty().WithErrorCode("3").WithMessage("Número do CPF deve ser informado");
        }
    }
}