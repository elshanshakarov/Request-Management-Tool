using Business.Abstract;
using Business.Constants;
using Core.Utilities.Results;
using Entities.Dto;
using Entities.Dto.Request;
using Entities.Dto.Response;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using IResult = Core.Utilities.Results.IResult;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("{id}")]
        public IActionResult GetUserById(int id)
        {
            IDataResult<RespUserDto> result = _userService.GetUserById(id);
            return Ok(result);
        }

        [HttpGet]
        public IActionResult GetAllUser()
        {
            IDataResult<List<RespUserDto>> result = _userService.GetAllUsers();
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest();
        }

        [HttpPut]
        public IActionResult UpdateUser(ReqUserDto userDto)
        {
            IResult result = _userService.Update(userDto);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            IResult result = _userService.Delete(id);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }



        [HttpPost("UpdateProfile")]
        public IActionResult UpdateProfile(UserDto userDto)
        {
            var userId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
            if (userId == 0)
            {
                return BadRequest(new ErrorResult(Messages.UserNotFound));
            }
            IResult result = _userService.UpdateProfile(userDto, userId);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("UpdatePassword")]
        public IActionResult UpdatePassword(string currentPassword, string newPassword, string againNewPassword)
        {
            var userId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
            if (userId == 0)
            {
                return BadRequest(new ErrorResult(Messages.UserNotFound));
            }
            IResult result = _userService.UpdatePassword(userId, currentPassword, newPassword, againNewPassword);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("UpdateImage")]
        public IActionResult UpdateImage(IFormFile file)
        {
            var userId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
            if (userId == 0)
            {
                return BadRequest(new ErrorResult(Messages.UserNotFound));
            }
            IResult result = _userService.UpdateImage(file, userId);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("DeleteImage")]
        public IActionResult DeleteImage()
        {
            var userId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
            if (userId == 0)
            {
                return BadRequest(new ErrorResult(Messages.UserNotFound));
            }
            var result = _userService.DeleteImage(userId);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        
    }
}
