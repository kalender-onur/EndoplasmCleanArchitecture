using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using EndoplasmCleanArchitecture.Authentication.Interfaces;
using EndoplasmCleanArchitecture.Authentication.Models;
using EndoplasmCleanArchitecture.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using EndoplasmCleanArchitecture.Application.Interfaces.User;

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;
    private readonly IAppUserService _appUserService;
    private readonly IUserService _userService;
    public TokenService(IConfiguration configuration, IAppUserService appUserService, IUserService userService)
    {
        _configuration = configuration;
        _appUserService = appUserService;
        _userService = userService;
    }

    public async Task<TokenResult> GenerateToken(AppUser user)
    {

        var jwtSettings = _configuration.GetSection("JWT").Get<JWT>();

        var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user?.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };


        var token = CreateToken(authClaims);
        var refreshToken = GenerateRefreshToken();


        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(jwtSettings.DurataionInDay);
        var tokenResult = new TokenResult
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            RefreshToken = refreshToken,
            Expiration = token.ValidTo
        };
        await SaveToken(user, tokenResult);
        return tokenResult;

    }

    private JwtSecurityToken CreateToken(List<Claim> authClaims)
    {
        var jwtSettings = _configuration.GetSection("JWT").Get<JWT>();

        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key));

        var token = new JwtSecurityToken(
            issuer: jwtSettings.Issuer,
            audience: jwtSettings.Audience,
            expires: DateTime.Now.AddDays(jwtSettings.DurataionInDay),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

        return token;
    }
    private async Task SaveToken(AppUser user, TokenResult tokenResult)
    {
        await _userService.InsertRefreshToken(user.UserName, tokenResult.RefreshToken, tokenResult.Expiration);
    }
    public async Task<TokenResult> RefreshTokensAsync(RefreshTokenRequest tokenModel)
    {

        string? accessToken = tokenModel.Token;
        string? refreshToken = tokenModel.RefreshToken;

        var principal = GetPrincipalFromExpiredToken(accessToken);
        if (principal == null)
        {
            return new TokenResult
            {
                ErrorMessage = "Invalid access token or refresh token"
            };
        }

        string username = principal.Identity.Name;
        if (username == null)
        {
            return new TokenResult
            {
                ErrorMessage = "username is null"
            };
        }
        var user = await _appUserService.GetUserByName(username);

        if (user == null)
        {
            return new TokenResult
            {
                ErrorMessage = "Invalid user."
            };
        }
        if (user.RefreshToken != refreshToken)
        {
            return new TokenResult
            {
                ErrorMessage = "Invalid access token or refresh token."
            };
        }
        if (user.RefreshTokenExpiryTime <= DateTime.Now)
        {
            return new TokenResult
            {
                ErrorMessage = "Refresh token expired."
            };
        }

        var newAccessToken = CreateToken(principal.Claims.ToList());
        var newRefreshToken = GenerateRefreshToken();

        user.RefreshToken = newRefreshToken;

        var tokenResult = new TokenResult
        {
            Token = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
            RefreshToken = newRefreshToken,
            Expiration = newAccessToken.ValidTo
        };

        await SaveToken(user, tokenResult);
        return tokenResult;

    }
    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    private ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token)
    {

        var jwtSettings = _configuration.GetSection("JWT").Get<JWT>();

        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key)),
            ValidateLifetime = false
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
        if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            throw new SecurityTokenException("Invalid token");

        return principal;

    }

}