using Business.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HistoriesController : ControllerBase
    {
        private readonly IHistoryService _historyService;

        public HistoriesController(IHistoryService historyService)
        {
            _historyService = historyService;
        }

        [HttpGet("GetAllHistoriesByRequestId")]
        public IActionResult GetAllHistoriesByRequestId(int requestId)
        {
            var result = _historyService.GetHistoryOfRequestById(requestId);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
