using ChatApp.Application.Common.Models;
using ChatApp.Application.Common.Models.User;
using FluentValidation;

namespace ChatApp.Application.Common.Validation.User;

/// <summary>
/// Represents a validator for the UserUpdateModel.
/// </summary>
public class UserUpdateModelValidator : AbstractValidator<UserUpdateModel>
{
    public UserUpdateModelValidator()
    {
        RuleFor(u => u.FirstName)
            .NotEmpty()
            .WithMessage("First name is required.")
            .MinimumLength(2)
            .WithMessage("First name must be at least 2 characters.")
            .MaximumLength(20)
            .WithMessage("First name can't exceed 20 characters.");

        RuleFor(u => u.LastName)
            .NotEmpty()
            .WithMessage("Last name is required.")
            .MinimumLength(2)
            .WithMessage("Last name must be at least 2 characters.")
            .MaximumLength(20)
            .WithMessage("Last name can't exceed 20 characters.");

        RuleFor(u => u.Email)
            .NotEmpty()
            .WithMessage("Email is required.")
            .EmailAddress()
            .WithMessage("Provided email is not valid.");

        RuleFor(u => u.About)
            .MaximumLength(100)
            .WithMessage("About field can't exceed 100 characters");

        RuleFor(u => u.Password)
            .NotEmpty()
            .WithMessage("Password is required.")
            .MinimumLength(5)
            .WithMessage("Password must be at least 5 characters.")
            .MaximumLength(20)
            .WithMessage("Password can't exceed 30 characters.");
    }
}