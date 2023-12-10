using Core.Dtos.JWT;
using Microsoft.AspNetCore.Identity;

namespace Core.Abstractions
{
    public interface IAuthService
    {
        Task<AuthDto> RegisterAsync(RegisterDto registerDto);
        Task<AuthDto> GetTokenLoginAsync(TokenRequestDto tokenRequestDto);
        Task<string> AddRoletoUserAsync(AddRoleDto addroleDto);

        Task<string> ChangePassowrdAsync(PayloadDTO payload, ChangePasswordDto changeDto);

    }
}
