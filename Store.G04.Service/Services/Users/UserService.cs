using Microsoft.AspNetCore.Identity;
using Store.G04.Core.Dtos.Auth;
using Store.G04.Core.Entities.Identity;
using Store.G04.Core.Repositories.Contract;
using Store.G04.Core.Services.Contract;

namespace Store.G04.Service.Services.Users;

public class UserService : IUserService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly ITokenService _tokenService;
    private readonly IEmailServices _emailServices;
    public UserService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ITokenService tokenService, IEmailServices emailServices)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
        _emailServices = emailServices;
    }

    /// <summary>
    /// Logs in a user with the provided credentials.
    /// </summary>
    /// <param name="loginDto">Login data transfer object containing email and password.</param>
    /// <returns>UserDto with user details and token, or null if login fails.</returns>
    public async Task<UserDto> LoginAsync(LoginDto loginDto)
    {
        var user = await _userManager.FindByEmailAsync(loginDto.Email);
        if (user is null) return null;

        var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
        if (!result.Succeeded) return null;

        return new UserDto
        {
            DisplayName = user.DisplayName,
            Email = user.Email,
            Token = await _tokenService.CreateTokenAsync(user, _userManager)
        };
    }

    /// <summary>
    /// Registers a new user.
    /// </summary>
    /// <param name="registerDto">Register data transfer object containing user details.</param>
    /// <returns>UserDto with user details and token, or null if registration fails.</returns>
    public async Task<UserDto> RegisterAsync(RegisterDto registerDto)
    {
        if (await CheckEmailExitsAsync(registerDto.Email)) return null;

        var user = new AppUser
        {
            Email = registerDto.Email,
            DisplayName = registerDto.DisplayName,
            PhoneNumber = registerDto.PhoneNumber,
            UserName = registerDto.Email.Split("@")[0]
        };

        var result = await _userManager.CreateAsync(user, registerDto.Password);
        if (!result.Succeeded) return null;

        return new UserDto
        {
            Email = user.Email,
            DisplayName = user.DisplayName,
            Token = await _tokenService.CreateTokenAsync(user, _userManager)
        };
    }

    /// <summary>
    /// Checks if an email is already registered.
    /// </summary>
    /// <param name="email">The email to check.</param>
    /// <returns>True if the email exists, otherwise false.</returns>
    public async Task<bool> CheckEmailExitsAsync(string email)
    {
        return await _userManager.FindByEmailAsync(email) is not null;
    }

    public async Task<EntityStatusDto> ForgetPasswordByEmailAsync(ForgetPasswordRequestByEmailDto forgetPasswordRequestDto)
    {
        try
        {
            var User = await _userManager.FindByEmailAsync(forgetPasswordRequestDto.Email);
            if (User is null) return null;
            var ResetCode = new Random().Next(100_000, 999_999);
            var ResetCodeExpiry = DateTime.UtcNow.AddMinutes(15);
            User.EmailConfirmResetCode = ResetCode;
            User.EmailConfirmResetCodeExpiry = ResetCodeExpiry;
            var Result = await _userManager.UpdateAsync(User);
            if (!Result.Succeeded) return null;
            var Email = new EmailDto()
            {
                To = forgetPasswordRequestDto.Email,
                Subject = "Reset Code For ECommerce Account",
                Body = $"We Have Recived Your Request For Reset Your Account Password, \nYour Reset Code Is ==> [ {ResetCode} ] <== \nNote: This Code Will Be Expired After 15 Minutes!"
            };
            await _emailServices.SendEmail(Email);
            EntityStatusDto SuccessObj = new EntityStatusDto()
            {
                Status = "Success",
                Message = "We Have Sent You The Reset Code"
            };
            return SuccessObj;
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public async Task<EntityStatusDto> VerifyCodeByEmailAsync(ResetCodeConfiramtionByEmailDto codeConfirmationDto)
    {
        var User = await _userManager.FindByEmailAsync(codeConfirmationDto.Email);
        if (User is null) return null;
        if (codeConfirmationDto.ResetCode != User.EmailConfirmResetCode) return null;
        if (User.EmailConfirmResetCodeExpiry < DateTime.UtcNow) return null;
        var SuccessObj = new EntityStatusDto()
        {
            Status = "Success",
            Message = "Reset Code Is Verified, Please Proceed To Change Your Password"
        };
        return SuccessObj;
    }

    public async Task<UserDto> ResetPasswordAsync(ResetPasswordDto resetPasswordDto)
    {
        var User = await _userManager.FindByEmailAsync(resetPasswordDto.Email);
        if (User is null) return null;
        var RemovedPassword = await _userManager.RemovePasswordAsync(User);
        if (!RemovedPassword.Succeeded) return null;
        var EnterNewPassword = await _userManager.AddPasswordAsync(User, resetPasswordDto.NewPassword);
        if (!EnterNewPassword.Succeeded) return null;
        var MappedUser = new UserDto()
        {
            DisplayName = User.DisplayName,
            Email = User.Email!,
            Token = await _tokenService.CreateTokenAsync(User, _userManager)
        };
        return MappedUser;
    }

}