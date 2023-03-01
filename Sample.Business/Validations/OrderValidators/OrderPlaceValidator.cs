using FluentValidation;
using Sample.Business.Dtos.OrderDtos;

namespace Sample.Business.Validations.OrderValidators;

public class OrderPlaceValidator : AbstractValidator<OrderAddDto> {
    public OrderPlaceValidator() {

        RuleFor(dto => dto.CustomerId)
            .SetValidator(new IdValidator("Customer ID"));
    }
}