using Business.Abstract;
using Entities.Dto;
using Entities.Dto.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public ActionResult Register(UserForRegisterDto userForRegisterDto)
        {
            var registerResult = _authService.Register(userForRegisterDto);
            if (!registerResult.Success)
            {
                return BadRequest(registerResult.Message);
            }

            var tokenResult = _authService.CreateAccessToken(registerResult.Data);
            if (!tokenResult.Success)
            {
                return BadRequest(tokenResult.Message);
            }

            return Ok(tokenResult.Data);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public ActionResult Login(UserForLoginDto userForLoginDto)
        {
            var userToLogin = _authService.Login(userForLoginDto);
            if (!userToLogin.Success)
            {
                return BadRequest(userToLogin.Message);
            }

            var result = _authService.CreateAccessToken(userToLogin.Data);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return Ok(result.Data);
        }

        [HttpPost("ForgotPassword")]
        [AllowAnonymous]
        public IActionResult ForgotPassword(string mail)
        {
            var result = _authService.ForgotPassword(mail);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("ResetPassword")]
        [AllowAnonymous]
        public IActionResult ResetPassword(ResetPasswordDto resetPassword)
        {
            var result = _authService.ResetPassword(resetPassword);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

    }
}
