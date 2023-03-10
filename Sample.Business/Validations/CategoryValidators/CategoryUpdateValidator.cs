using FluentValidation;
using Sample.Business.Dtos.CategoryDtos;

namespace Sample.Business.Validations.CategoryValidators;

public class CategoryUpdateValidator : AbstractValidator<CategoryDto> {
    public CategoryUpdateValidator() {

        RuleFor(x => x.Id)
            .SetValidator(new IdValidator());

        Include(new CategoryAddValidator());
    }
}