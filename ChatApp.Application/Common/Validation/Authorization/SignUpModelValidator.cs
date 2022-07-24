using ChatApp.Application.Common.Models.Authorization;
using FluentValidation;

namespace ChatApp.Application.Common.Validation.Authorization;

/// <summary>
/// Represents a validator for the SignUpModel. 
/// </summary>
public class SignUpModelValidator : AbstractValidator<SignUpModel>
{
    public SignUpModelValidator()
    {
        RuleFor(s => s.UserName)
            .NotEmpty()
            .WithMessage("Username is required.")
            .MinimumLength(5)
            .WithMessage("Username must be at least 5 characters.")
            .MaximumLength(30)
            .WithMessage("Username can't exceed 30 characters.");

        RuleFor(s => s.Password)
            .NotEmpty()
            .WithMessage("Password is required.")
            .MinimumLength(5)
            .WithMessage("Password must be at least 5 characters.")
            .MaximumLength(20)
            .WithMessage("Password can't exceed 30 characters.");

        RuleFor(s => s.Email)
            .NotEmpty()
            .WithMessage("Email is required.")
            .EmailAddress()
            .WithMessage("Provided email is not valid.");

        RuleFor(s => s.FirstName)
            .NotEmpty()
            .WithMessage("First name is required.")
            .MinimumLength(2)
            .WithMessage("First name must be at least 2 characters.")
            .MaximumLength(20)
            .WithMessage("First name can't exceed 20 characters.");
        
        RuleFor(s => s.LastName)
            .NotEmpty()
            .WithMessage("Last name is required.")
            .MinimumLength(2)
            .WithMessage("Last name must be at least 2 characters.")
            .MaximumLength(20)
            .WithMessage("Last name can't exceed 20 characters.");

        RuleFor(s => s.BirthDate)
            .NotEmpty()
            .WithMessage("Birth date is required.");
    }
}