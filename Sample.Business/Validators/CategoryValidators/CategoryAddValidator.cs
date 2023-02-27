using FluentValidation;
using Sample.Business.Dtos.CategoryDtos;

namespace Sample.Business.Validators.CategoryValidators;

public class CategoryAddValidator : AbstractValidator<CategoryAddDto> {
    public CategoryAddValidator() {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Description).NotEmpty();
    }
}