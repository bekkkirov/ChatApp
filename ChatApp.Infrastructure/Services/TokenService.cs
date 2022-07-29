using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using ChatApp.Application.Common.Exceptions;
using ChatApp.Application.Common.Interfaces;
using ChatApp.Application.Common.Models.Authorization;
using ChatApp.Application.Common.Settings;
using ChatApp.Infrastructure.Identity.Entities;
using ChatApp.Infrastructure.Identity.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace ChatApp.Infrastructure.Services;

///<inheritdoc cref="ITokenService"/>
public class TokenService : ITokenService
{
    private readonly SymmetricSecurityKey _key;
    private readonly TokenValidationParameters _tokenValidationParameters;
    private readonly UserManager<UserIdentity> _userManager;

    /// <summary>
    /// Creates an instance of the token service.
    /// </summary>
    public TokenService(IOptions<TokenSettings> tokenSettings, TokenValidationParameters tokenValidationParameters, UserManager<UserIdentity> userManager)
    {
        _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenSettings.Value.Key));
        _tokenValidationParameters = tokenValidationParameters;
        _userManager = userManager;
    }

    public string GenerateAccessToken(string userName)
    {
        var claims = new List<Claim>()
        {
            new Claim(JwtRegisteredClaimNames.NameId, userName)
        };

        var credentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddDays(14),
            SigningCredentials = credentials
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];

        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
            var refreshToken = Convert.ToBase64String(randomNumber);

            return refreshToken.Substring(0, refreshToken.Length > 64 ? 64 : refreshToken.Length);
        }
    }

    public async Task<string> RefreshToken(string userName, TokensModel tokens)
    {
        var user = await _userManager.FindByNameWithRefreshTokenAsync(userName);

        if (user is null)
        {
            throw new ValidationException("User with the specified username doesn't exist.");
        }

        if (user.RefreshToken is null)
        {
            throw new ValidationException("User doesn't have generated refresh token.");
        }

        if (user.RefreshToken.Token != tokens.RefreshToken)
        {
            throw new ValidationException("Provided refresh token doesn't belong to this user.");
        }

        if (user.RefreshToken.IsUsed)
        {
            throw new ValidationException("Refresh token has been already used.");
        }

        if (user.RefreshToken.ExpiryDate <= DateTime.UtcNow)
        {
            throw new ValidationException("Refresh token has already expired.");
        }

        user.RefreshToken.IsUsed = true;
        await _userManager.UpdateAsync(user);

        var principal = GetPrincipalFromExpiredToken(tokens.AccessToken);

        return GenerateAccessToken(principal.FindFirstValue(ClaimTypes.NameIdentifier));
    }

    public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        var principal = tokenHandler.ValidateToken(token, _tokenValidationParameters, out var securityToken);
        var jwtSecurityToken = securityToken as JwtSecurityToken;

        if (jwtSecurityToken == null)
        {
            throw new ValidationException("Provided token is invalid.");
        }
            
        return principal;
    }
}