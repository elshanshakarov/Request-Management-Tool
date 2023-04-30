using Business.Abstract;
using Core.Utilities.Results;
using Entities.Concrete;
using Entities.Dto;
using Entities.Dto.Request;
using Entities.Dto.Response;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;


namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestsController : ControllerBase
    {
        private readonly IRequestService _requestService;
        private readonly IHttpContextAccessor _contextAccessor;


        public RequestsController(IRequestService requestService)
        {
            _requestService = requestService;
        }

        [HttpPost]
        public IActionResult AddRequest(ReqRequestDto requestDto)
        {
            var userId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);

            Request request = new Request
            {
                CreatorId = userId,
                CategoryId = requestDto.CategoryId,
                PriorityId = requestDto.PriorityId,
                RequestTypeId = requestDto.RequestTypeId,
                Tittle = requestDto.Tittle,
                Text = requestDto.Text,
                StatusId = requestDto.StatusId,
            };

            var result = _requestService.Add(request);

            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }


        [HttpGet("GetAllRequests")]
        public IActionResult GetAllRequests(RequestFilterDto requestFilterDto)
        {
            var userId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
            IDataResult<List<RespRequestDto>> result = _requestService.GetAllFilteredRequests(userId, requestFilterDto);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpGet("GetAllMyRequests")]
        public IActionResult GetAllMyRequests(short? statusId)
        {
            var userId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
            var result = _requestService.GetAllMyRequests(userId, statusId);

            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpGet("GetCountOfAllRequests")]
        public IActionResult GetCountOfAllRequests(RequestFilterDto requestFilterDto)
        {
            var userId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
            var result = _requestService.GetCountOfAllRequests(userId, requestFilterDto);

            if (result.Data.Count == 0)
            {
                return Ok(result.Data.Count);
            }
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpGet("GetCountOfAllMyRequests")]
        public IActionResult GetCountOfAllMyRequests(short? statusId)
        {
            var userId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
            var result = _requestService.GetCountOfAllMyRequests(userId, statusId);

            if (result.Data.Count == 0)
            {
                return Ok(result.Data.Count);
            }
            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpGet("GetRequestByIdRequestDto")]
        public IActionResult GetRequestByIdRequestDto(int requestId)
        {
            var userId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
            var result = _requestService.GetRequestByIdRequestDto(userId, requestId);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpGet("GetRequestByIdCommentDto")]
        public IActionResult GetRequestByIdCommentDto(int requestId)
        {
            var userId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
            var result = _requestService.GetRequestByIdCommentDto(userId, requestId);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPost("AddComment")]
        public IActionResult AddComment(int requestId, string commentText)
        {
            var userId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
            var result = _requestService.AddComment(userId, requestId, commentText);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpGet("ChangeStatus")]
        public IActionResult ChangeStatus(int requestId, short statusId)
        {
            var userId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
            var result=_requestService.ChangeStatus(requestId, userId, statusId);
            return null;
        }







        /*--------------------------------------------------------------------------------------------------------------------*/
        [HttpDelete("{id}")]
        public IActionResult DeleteRequest(int id)
        {
            var result = _requestService.Delete(id);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPut]
        public IActionResult UpdateRequest(ReqRequestDto reqRequestDto)
        {
            var result = _requestService.Update(reqRequestDto);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

    }
}
