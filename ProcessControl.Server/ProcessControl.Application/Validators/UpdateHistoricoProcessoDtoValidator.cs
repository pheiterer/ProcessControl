using FluentValidation;
using ProcessControl.Application.DTOs;

namespace ProcessControl.Application.Validators
{
    public class UpdateHistoricoProcessoDtoValidator : AbstractValidator<UpdateHistoricoProcessoDto>
    {
        public UpdateHistoricoProcessoDtoValidator()
        {
            RuleFor(x => x.Descricao)
                .NotEmpty().WithMessage("A descrição é obrigatória.");
        }
    }
}
