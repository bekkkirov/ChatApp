using ChatApp.Application.Common.Models.Authorization;
using FluentValidation;

namespace ChatApp.Application.Common.Validation.Authorization;

/// <summary>
/// Represents a validator for the TokensModel. 
/// </summary>
public class TokensModelValidator : AbstractValidator<TokensModel>
{
    public TokensModelValidator()
    {
        RuleFor(t => t.AccessToken)
            .NotEmpty()
            .WithMessage("Access token is required.");

        RuleFor(t => t.RefreshToken)
            .NotEmpty()
            .WithMessage("Refresh token is required.");
    }
}