using Business.Abstract;
using Core.Utilities.Results;
using Core.Utilities.Security.JWT;
using Entities.Dto.Request;
using Entities.Dto.Response;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
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



        //[HttpPost]
        //public IActionResult AddUser(ReqUserDto userDto)
        //{
        //    // User user = _mapper.Map<User>(userDto);
        //    IResult result = _userService.Add(userDto);
        //    if (result.Success)
        //    {
        //        return Ok(result);
        //    }
        //    return BadRequest(result);
        //}

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




        [HttpGet("/currentId")]
        public IActionResult current()
        {
            var claims = HttpContext.User.Claims;
            int id = Convert.ToInt32(claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);

            return Ok(new { userId = id });
        }
    }
}
