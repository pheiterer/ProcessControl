using FluentValidation;
using ProcessControl.Application.DTOs;

namespace ProcessControl.Application.Validators
{
    public class CreateProcessoDtoValidator : AbstractValidator<CreateProcessoDto>
    {
        public CreateProcessoDtoValidator()
        {
            RuleFor(x => x.NumeroProcesso)
                .NotEmpty().WithMessage("O número do processo é obrigatório.")
                .MaximumLength(100).WithMessage("O número do processo não pode exceder 100 caracteres.");

            RuleFor(x => x.Autor)
                .NotEmpty().WithMessage("O autor é obrigatório.")
                .MaximumLength(100).WithMessage("O autor não pode exceder 100 caracteres.");

            RuleFor(x => x.Reu)
                .NotEmpty().WithMessage("O réu é obrigatório.")
                .MaximumLength(100).WithMessage("O réu não pode exceder 100 caracteres.");
        }
    }
}
