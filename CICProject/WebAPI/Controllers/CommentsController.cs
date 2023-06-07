using Business.Abstract;
using Business.Constants;
using Core.Utilities.Results;
using Entities.Dto;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentsController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpGet("GetAllCommentsByRequestId")]
        public IActionResult GetAllCommentsByRequestId(int requestId)
        {
            int userId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
            if (userId == 0)
            {
                return BadRequest(new ErrorResult(Messages.UserNotFound));
            }
            var result = _commentService.GetAllCommentsByRequestId(userId, requestId);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPost("AddComment")]
        public IActionResult AddComment([FromForm] ReqCommentDto reqComment)
        {
            var userId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
            if (userId == 0)
            {
                return BadRequest(new ErrorResult(Messages.UserNotFound));
            }
            var result = _commentService.AddComment(reqComment, userId);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
