using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Core.Abstractions;
using Core.Dtos;
using Core.Dtos.JWT;
using Core.Entities;
using Infrastructure.Utility;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Application.Services.Authentiction_Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _UserManager;
        private readonly IMapper _mapper;
        private readonly JWT _JWT;
        private readonly RoleManager<IdentityRole> _RoleManager;
        private readonly IWebHostEnvironment _env;

        public AuthService(UserManager<ApplicationUser> userManager, IMapper mapper, IOptions<JWT> jWT, RoleManager<IdentityRole> roleManager, IWebHostEnvironment env)
        {
            _UserManager = userManager;
            _mapper = mapper;
            _JWT = jWT.Value;
            _RoleManager = roleManager;
            _env = env;
        }

        #region Register service
        public async Task<AuthDto> RegisterAsync(RegisterDto registerDto)
        {


            if (await _UserManager.FindByEmailAsync(registerDto.Email) is not null)
                return new AuthDto { Message = "Email is already registered" };

            if (await _UserManager.FindByNameAsync(registerDto.UserName) is not null)
                return new AuthDto { Message = "UserName is already registered" };

           var newuser= _mapper.Map<ApplicationUser>(registerDto);

            string wwwRootPath = _env.WebRootPath;
            if (registerDto.FormFile != null)
            {
                string imageFileName = Guid.NewGuid().ToString() + Path.GetExtension(registerDto.FormFile.FileName);
                string folderPath = Path.Combine(wwwRootPath, @"Images\Patients");

                using (var stream = new FileStream(Path.Combine(folderPath, imageFileName), FileMode.Create))
                {
                    registerDto.FormFile.CopyTo(stream);
                }
                registerDto.ImageUrl = @"\Images\Patients\" + imageFileName;
            }

            newuser.ImageUrl = registerDto.ImageUrl;
            newuser.Gender = registerDto.Gender.ToString();

            var result = await _UserManager.CreateAsync(newuser, registerDto.Password);
            if(!result.Succeeded)
            {
                var errors = string.Empty;
                foreach (var e in result.Errors)
                {
                    errors += $"{e.Description},";
                }
                return new AuthDto { Message = errors };
            }
           var userrole= await _UserManager.AddToRoleAsync(newuser, SD.ROLE_PATIENT);

            var JWTtoken = await Createtoken(newuser);

            return new AuthDto
            {
                Email = newuser.Email,
                ExpiresON = JWTtoken.ValidTo,
                IsAuthenticated = true,
                Roles=new List<string> { SD.ROLE_PATIENT},
                Token=new JwtSecurityTokenHandler().WriteToken(JWTtoken),
                Username=newuser.UserName
            };
        }
        #endregion

        #region login service
        public async Task<AuthDto> GetTokenLoginAsync(TokenRequestDto tokenRequestDto)
        {
            var authmodel = new AuthDto();
            var user = await _UserManager.FindByEmailAsync(tokenRequestDto.Email);

            if (user is null || !await _UserManager.CheckPasswordAsync(user, tokenRequestDto.Password))
            {
                authmodel.Message = "Email or password is incorrect";
                return authmodel;
            }
            var JWTtoken = await Createtoken(user);
            var rolesList = await _UserManager.GetRolesAsync(user);
            authmodel.IsAuthenticated = true;
            authmodel.Token = new JwtSecurityTokenHandler().WriteToken(JWTtoken);
            authmodel.Email=user.Email;
            authmodel.Username = user.UserName;
            authmodel.ExpiresON=JWTtoken.ValidTo;
            authmodel.Roles=rolesList.ToList();

            return authmodel;
        }
        #endregion

        #region Change Password Service
        public async Task<string> ChangePassowrdAsync(PayloadDTO payload, ChangePasswordDto changePasswordDto)
        {
            var user = await _UserManager.FindByEmailAsync(payload.Email);

            if (!await _UserManager.CheckPasswordAsync(user, changePasswordDto.CurrentPassword))
            {
                return "wrong Password";
            }
            var Result = await _UserManager.ChangePasswordAsync(user, changePasswordDto.CurrentPassword, changePasswordDto.NewPassword);

            if (!Result.Succeeded)
            {
                var errors = string.Empty;
                foreach (var e in Result.Errors)
                {
                    errors += $"{e.Description},";
                }
                return errors;
            }

            return string.Empty;

        }

        #endregion

        #region add role to user Service
        public async Task<string> AddRoletoUserAsync(AddRoleDto addRoleDto)
        {
            var user = await _UserManager.FindByIdAsync(addRoleDto.UserId);

            if (user is null || !await _RoleManager.RoleExistsAsync(addRoleDto.RoleName))
                return "Invalid User ID or Role";

            if (await _UserManager.IsInRoleAsync(user, addRoleDto.RoleName))
                return "user already assigned to this role";

            var result = await _UserManager.AddToRoleAsync(user, addRoleDto.RoleName);

            return result.Succeeded? string.Empty : "Something went wrong";

        }
        #endregion


        #region Create JWT Token method
        private async Task<JwtSecurityToken> Createtoken(ApplicationUser user)
        {
            var userclaims = await _UserManager.GetClaimsAsync(user);

            var roles = await _UserManager.GetRolesAsync(user);

            var roleclaims = new List<Claim>();

            foreach (var role in roles)
                roleclaims.Add(new Claim("roles", role));

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub,user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()), 
                new Claim(JwtRegisteredClaimNames.Email,user.Email),
                new Claim("uid",user.Id),
                new Claim("Name", user.FirstName + user.LastName),
            }.Union(userclaims).Union(roleclaims);

            var symmetricsecuritykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_JWT.Key));

            var signingcredentials = new SigningCredentials(symmetricsecuritykey, SecurityAlgorithms.HmacSha256);
            var jwtsecuritytoken = new JwtSecurityToken(
                issuer: _JWT.Issuer,
                audience: _JWT.Audience,
                claims: claims,
                expires: DateTime.Now.AddDays(_JWT.DurationInDays),
                signingCredentials: signingcredentials);

            return jwtsecuritytoken;
        }
        #endregion
    }
}
