using FluentValidation;
using Sample.Business.Dtos.OrderDtos;

namespace Sample.Business.Validations.OrderValidators;

public class OrderItemAddValidator : AbstractValidator<OrderItemAddDto> {
    public OrderItemAddValidator() {
        RuleFor(x => x.FoodId)
            .GreaterThan(0).WithMessage("Invalid Food ID");

        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("Quantity should be greater than 0");
    }
}