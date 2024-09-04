using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using EndoplasmCleanArchitecture.Api.DTOs;
using EndoplasmCleanArchitecture.Api.DTOs.Account;
using EndoplasmCleanArchitecture.Authentication.Interfaces;
using EndoplasmCleanArchitecture.Authentication.Models;
using EndoplasmCleanArchitecture.Application.Interfaces.User;
using EndoplasmCleanArchitecture.Domain.Entities;

namespace EndoplasmCleanArchitecture.Api.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAppUserService _appUserService;
        private readonly ITokenService _tokenService;

        public AuthController(IAppUserService appUserService, ITokenService tokenService)
        {
            _appUserService = appUserService;
            _tokenService = tokenService;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var result = await _appUserService.Authenticate(request.Username, request.Password);
            if (result != null && result.isSuccess && result.data != null)
            {
                var token = await _tokenService.GenerateToken(result.data);
                return Ok(Response<TokenResult>.SuccessResponse(token));
            }

            return Unauthorized(Response<string>.ErrorResponse(result?.errorMessage ?? "Unauthorized", 401));

        }

        [HttpPost("refreshToken")]
        public async Task<IActionResult> RefreshTokenAsync([FromBody] RefreshTokenRequest refreshTokenRequest)
        {
            try
            {
                var result = await _tokenService.RefreshTokensAsync(refreshTokenRequest);
                return Ok(Response<TokenResult>.SuccessResponse(result));
            }
            catch (SecurityTokenException ex)
            {
                return BadRequest(Response<string>.ErrorResponse($"Invalid token : {ex.Message}", 500));

            }
            catch (Exception ex)
            {
                return BadRequest(Response<string>.ErrorResponse(ex.Message, 500));
            }
        }
    }

}