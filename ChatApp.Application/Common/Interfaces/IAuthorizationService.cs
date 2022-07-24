using ChatApp.Application.Common.Models.Authorization;

namespace ChatApp.Application.Common.Interfaces;

public interface IAuthorizationService
{
    public Task<TokensModel> SignInAsync(SignInModel signInData);

    public Task<TokensModel> SignUpAsync(SignUpModel signUpData);
}