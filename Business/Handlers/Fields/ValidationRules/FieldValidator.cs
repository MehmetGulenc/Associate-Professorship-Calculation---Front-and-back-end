
using Business.Handlers.Fields.Commands;
using FluentValidation;

namespace Business.Handlers.Fields.ValidationRules
{

    public class CreateFieldValidator : AbstractValidator<CreateFieldCommand>
    {
        public CreateFieldValidator()
        {
            RuleFor(x => x.Name).NotEmpty();

        }
    }
    public class UpdateFieldValidator : AbstractValidator<UpdateFieldCommand>
    {
        public UpdateFieldValidator()
        {
            RuleFor(x => x.Name).NotEmpty();

        }
    }
}