using FluentValidation;
using Sample.Business.Dtos.FoodDtos;

namespace Sample.Business.Validations.FoodValidators;

public class FoodUpdateValidator : AbstractValidator<FoodDto> {
    public FoodUpdateValidator() {

        RuleFor(x => x.Id)
            .SetValidator(new IdValidator());

        Include(new FoodAddValidator());
    }
}