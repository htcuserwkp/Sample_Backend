using FluentValidation;

namespace Sample.Business.Validations;

public class IdValidator : AbstractValidator<long>{
    public IdValidator(string idName = "ID") {

        RuleFor(x => x)
            .NotEmpty().WithMessage($"{idName} is required")
            .GreaterThan(0).WithMessage($"Invalid {idName}");
    }
}