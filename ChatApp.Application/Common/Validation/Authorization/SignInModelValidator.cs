using ChatApp.Application.Common.Models.Authorization;
using FluentValidation;

namespace ChatApp.Application.Common.Validation.Authorization;

/// <summary>
/// Represents a validator for the SignInModel. 
/// </summary>
public class SignInModelValidator : AbstractValidator<SignInModel>
{
    public SignInModelValidator()
    {
        RuleFor(s => s.UserName)
            .NotEmpty()
            .WithMessage("Username is required.");

        RuleFor(s => s.Password)
            .NotEmpty()
            .WithMessage("Password is required.");
    }
}