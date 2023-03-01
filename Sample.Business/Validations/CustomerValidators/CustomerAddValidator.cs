using FluentValidation;
using Sample.Business.Dtos.CustomerDtos;

namespace Sample.Business.Validations.CustomerValidators;

public class CustomerAddValidator : AbstractValidator<CustomerAddDto> {
    public CustomerAddValidator() {

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(50)
            .WithMessage("Enter a valid name");

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .WithMessage("Enter a valid email");

        RuleFor(x => x.Phone)
            .NotEmpty()
            .Matches(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$")
            .WithMessage("Enter a valid phone number");
    }
}