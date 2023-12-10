using Core.Abstractions;
using Core.Dtos.JWT;
using Infrastructure.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        #region Register endpoint
        [HttpPost("Register")]
        public async Task<IActionResult> RegisterAsync([FromForm] RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.RegisterAsync(registerDto);
            if (!result.IsAuthenticated)
                return BadRequest(result.Message);

            return Ok(result);
        }
        #endregion

        #region login endpoint
        [HttpPost("Login")]
        public async Task<IActionResult> LoginAsync([FromBody] TokenRequestDto loginModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.GetTokenLoginAsync(loginModel);

            if (!result.IsAuthenticated)
                return BadRequest(result.Message);

            return Ok(result);
        }

        #endregion

        #region Change Password endpoint
        [HttpPost("Change Password")]
        public async Task<IActionResult> ChangePassword([FromBody]ChangePasswordDto changemodel)
        {
            // Get the token from Headers.
            var token = HttpContext.Request.Headers["Authorization"].ToString().Split("Bearer ");

            // Here we need to extract all of data from the token.
            var Payload = ReadJWTToken.ExtractPayload(token[1].ToString());

            // Mapping 
            var Data = JsonConvert.DeserializeObject<PayloadDTO>(Payload, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

            var result = await _authService.ChangePassowrdAsync(Data,changemodel);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!string.IsNullOrEmpty(result))
                return BadRequest(result);

            return Ok("Password Changed SuccessFully");
        }
        #endregion

        #region add role to user EndPoint
        [HttpPost("AddRole")]
        public async Task<IActionResult> AddRoleAsync([FromBody] AddRoleDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.AddRoletoUserAsync(model);

            if (!string.IsNullOrEmpty(result))
                return BadRequest(result);

            return Ok(model);

         }
        #endregion

 

    }
}
