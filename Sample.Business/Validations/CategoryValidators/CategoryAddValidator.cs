using FluentValidation;
using Sample.Business.Dtos.CategoryDtos;

namespace Sample.Business.Validations.CategoryValidators;

public class CategoryAddValidator : AbstractValidator<CategoryAddDto> {
    public CategoryAddValidator() {

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required");

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Description is required");
    }
}