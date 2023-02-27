using FluentValidation;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Sample.Business.Dtos;

namespace Sample.Business.Validators;

public class IdValidator : AbstractValidator<long>{
    public IdValidator() {

        RuleFor(x => x)
            .NotEmpty().WithMessage("ID is required")
            .GreaterThan(0).WithMessage("Invalid ID");
    }
}