using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EndoplasmCleanArchitecture.Api.DTOs;
using EndoplasmCleanArchitecture.Application.Dtos.User;
using EndoplasmCleanArchitecture.Application.Interfaces.User;

namespace EndoplasmCleanArchitecture.Api.Controllers
{

    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class SampleController : ControllerBase
    {

        private readonly IUserService _userService;
        public SampleController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpGet("secure-data")]
        public IActionResult GetSecureData()
        {
            return Ok(Response<string>.SuccessResponse("This is a secure data."));
        }
        [HttpGet("exception-test")]
        public IActionResult ExceptionTest(int deger)
        {
            return Ok(Response<int>.SuccessResponse(deger / deger));
        }

        [HttpPost("createUser")]
        public async Task<IActionResult> User([FromBody] CreateUserRequestDto request)
        {
            await _userService.CreateUserAsync(request);
            return Ok(Response<bool>.SuccessResponse(true));
        }
    }

}
