using Store.G04.Core.Dtos.Auth;

namespace Store.G04.Core.Services.Contract;
public interface IUserService
{
    Task<UserDto> LoginAsync(LoginDto loginDto);
    Task<UserDto> RegisterAsync(RegisterDto registerDto);

    Task<bool> CheckEmailExitsAsync(string email);
    Task<EntityStatusDto> ForgetPasswordByEmailAsync(ForgetPasswordRequestByEmailDto forgetPasswordRequestDto);
    Task<EntityStatusDto> VerifyCodeByEmailAsync(ResetCodeConfiramtionByEmailDto codeConfirmationDto);
    Task<UserDto> ResetPasswordAsync(ResetPasswordDto resetPasswordDto);
}
