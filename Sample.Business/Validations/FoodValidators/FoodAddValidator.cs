using FluentValidation;
using Sample.Business.Dtos.FoodDtos;

namespace Sample.Business.Validations.FoodValidators;

public class FoodAddValidator : AbstractValidator<FoodAddDto> {
    public FoodAddValidator() {
        RuleFor(dto => dto.Name)
            .NotEmpty().WithMessage("Name is required");

        RuleFor(dto => dto.Description)
            .NotEmpty().WithMessage("Description is required");

        RuleFor(dto => dto.Quantity)
            .GreaterThanOrEqualTo(0).WithMessage("Quantity must be greater than or equal to 0");

        RuleFor(dto => dto.Price)
            .GreaterThan(0).WithMessage("Price must be greater than zero");

        RuleFor(dto => dto.CategoryId)
            .SetValidator(new IdValidator("Category ID"));
    }
}
