using Microsoft.AspNetCore.Mvc;
using Store.G04.APIs.Errors;
using Store.G04.Core.Dtos.Auth;
using Store.G04.Core.Services.Contract;

namespace Store.G04.APIs.Controllers;
public class AuthenticationController : BaseApiController
{
    private readonly IUserService _userService;

    public AuthenticationController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("Login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
    {
        var user = await _userService.LoginAsync(loginDto);
        if (user is null)
            return Unauthorized(new ApiErrorResponse(StatusCodes.Status401Unauthorized));

        return Ok(user);
    }

    [HttpPost("Register")] 
    public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
    {
        var user = await _userService.RegisterAsync(registerDto);
        if (user is null)
            return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest, "Invalid Registration !!"));

        return Ok(user);
    }

    [HttpPost("ForgetPasswordByEmail")]
    public async Task<ActionResult<EntityStatusDto>> ForgetPasswordByEmail(ForgetPasswordRequestByEmailDto forgetPasswordRequestDto)
    {
        var Result = await _userService.ForgetPasswordByEmailAsync(forgetPasswordRequestDto);
        return Ok(Result);
    }

    [HttpPost("ResetCodeByEmail")]
    public async Task<ActionResult<EntityStatusDto>> ResetCodeByEmail(ResetCodeConfiramtionByEmailDto resetCodeConfiramtionDto)
    {
        var Result = await _userService.VerifyCodeByEmailAsync(resetCodeConfiramtionDto);
        return Ok(Result);
    }

    [HttpPut("ResetPassword")]
    public async Task<ActionResult<UserDto>> ResetPassword(ResetPasswordDto resetPasswordDto)
    {
        var Result = await _userService.ResetPasswordAsync(resetPasswordDto);
        return Ok(Result);
    }
}