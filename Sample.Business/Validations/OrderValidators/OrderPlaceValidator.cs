using FluentValidation;
using Sample.Business.Dtos.OrderDtos;

namespace Sample.Business.Validations.OrderValidators;

public class OrderPlaceValidator : AbstractValidator<OrderAddDto> {
    public OrderPlaceValidator() {

        RuleFor(x => x.CustomerId)
            .SetValidator(new IdValidator("Customer ID"));

        RuleFor(x => x.OrderItems)
            .NotNull()
            .Must(x => x.Any()).WithMessage("OrderItems should not be empty")
            .ForEach(item => item.SetValidator(new OrderItemAddValidator()));
    }
}