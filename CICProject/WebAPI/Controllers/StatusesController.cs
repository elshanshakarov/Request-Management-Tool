using Business.Abstract;
using Business.Constants;
using Core.Utilities.Results;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatusesController : ControllerBase
    {
        private readonly IStatusService _statusService;

        public StatusesController(IStatusService statusService)
        {
            _statusService = statusService;
        }

        [HttpPost("UpdateStatus")]
        public IActionResult UpdateStatus(int requestId, short? statusId)
        {
            var userId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
            if (userId == 0)
            {
                return BadRequest(new ErrorResult(Messages.UserNotFound));
            }
            var result = _statusService.UpdateStatus(requestId, userId, statusId);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
