using FluentValidation;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Sample.Business.Dtos;

namespace Sample.Business.Validators;

public class IdValidator : AbstractValidator<long>{
    public IdValidator() {
        RuleFor(x => x)
            .NotEmpty()
            .GreaterThan(0);
    }
}