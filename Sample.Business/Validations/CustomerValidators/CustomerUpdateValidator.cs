using FluentValidation;
using Sample.Business.Dtos.CustomerDtos;

namespace Sample.Business.Validations.CustomerValidators;

public class CustomerUpdateValidator : AbstractValidator<CustomerDto> {
    public CustomerUpdateValidator() {

        RuleFor(x => x.Id)
            .SetValidator(new IdValidator());

        Include(new CustomerAddValidator());
    }
}