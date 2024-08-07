using Microsoft.IdentityModel.Tokens;
using EndoplasmCleanArchitecture.Api.DTOs;
using EndoplasmCleanArchitecture.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Text.Json;

namespace EndoplasmCleanArchitecture.Api.Middleware
{
    public class TokenValidationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;

        public TokenValidationMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token != null)
            {
                try
                {
                    var jwtSettings = _configuration.GetSection("JWT").Get<JWT>();
                    var key = Encoding.ASCII.GetBytes(jwtSettings.Key);
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var tokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false,
                        ValidateIssuer = false,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key)),
                        ValidateLifetime = false
                    };

                    var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken validatedToken);

                    var jwtToken = validatedToken as JwtSecurityToken;
                    if (jwtToken == null || !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                    {
                        context.Response.StatusCode = 401;
                        context.Response.ContentType = "application/json";

                        var res = Response<string>.ErrorResponse("Unauthorized", 401);
                        await context.Response.WriteAsync(JsonSerializer.Serialize(res));
                        return;
                    }

                    if (jwtToken.ValidTo < DateTime.UtcNow)
                    {
                        context.Response.StatusCode = 401;
                        context.Response.ContentType = "application/json";

                        var res = Response<string>.ErrorResponse("Token expired", 401);
                        await context.Response.WriteAsync(JsonSerializer.Serialize(res));
                        return;
                    }
                }
                catch
                {
                    context.Response.StatusCode = 401;
                    context.Response.ContentType = "application/json";

                    var res = Response<string>.ErrorResponse("Unauthorized", 401);
                    await context.Response.WriteAsync(JsonSerializer.Serialize(res));

                    return;
                }
            }

            await _next(context);
        }
    }
}
