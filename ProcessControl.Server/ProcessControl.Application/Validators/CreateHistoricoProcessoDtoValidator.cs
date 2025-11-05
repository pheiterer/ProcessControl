using FluentValidation;
using ProcessControl.Application.DTOs;

namespace ProcessControl.Application.Validators
{
    public class CreateHistoricoProcessoDtoValidator : AbstractValidator<CreateHistoricoProcessoDto>
    {
        public CreateHistoricoProcessoDtoValidator()
        {
            RuleFor(x => x.Descricao)
                .NotEmpty().WithMessage("A descrição é obrigatória.");
        }
    }
}
