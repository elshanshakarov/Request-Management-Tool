using Business.Abstract;
using ClosedXML.Excel;
using Core.Utilities.Results;
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

        public RequestsController(IRequestService requestService)
        {
            _requestService = requestService;
        }

        [HttpPost("AddRequest")]

        public IActionResult AddRequest([FromForm] ReqRequestDto requestDto)
        {
            var userId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
            var result = _requestService.AddRequest(requestDto, userId);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPost("UpdateRequest")]
        public IActionResult UpdateRequest(RequestInfoDto requestInfoDto)
        {
            var userId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
            var result = _requestService.UpdateRequest(requestInfoDto, userId);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpGet("GetRequestById")]
        public IActionResult GetRequestById(int requestId)
        {
            var userId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
            var result = _requestService.GetRequestById(userId, requestId);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpGet("GetAllRequests")]
        public IActionResult GetAllRequests(AllRequestFilterDto allRequestFilterDto)
        {
            var userId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);

            IDataResult<List<RespRequestDto>> result = _requestService.GetAllRequests(userId, allRequestFilterDto);
            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpGet("GetAllMyRequests")]
        public IActionResult GetAllMyRequests(MyRequestFilterDto myRequestFilterDto)
        {
            var userId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
            var result = _requestService.GetAllMyRequests(userId, myRequestFilterDto);

            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpGet("GetCountOfAllRequests")]
        public IActionResult GetCountOfAllRequests(AllRequestFilterDto requestFilterDto)
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

        [HttpGet("GetAllReports")]
        public IActionResult GetAllReports(ReportFilterDto reportFilterDto)
        {
            var userId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
            var result = _requestService.GetAllReports(reportFilterDto, userId);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpGet("TransferToExcel")]
        public IActionResult TransferToExcel(ExcelFilterDto excelFilterDto)
        {
            var userId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
            var result = _requestService.GetAllExcelReports(excelFilterDto, userId);
            if (result.Success)
            {
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add();
                    var currentRowCount = 1;
                    worksheet.Cell(currentRowCount, 1).Value = "ID";
                    worksheet.Cell(currentRowCount, 2).Value = "Creator";
                    worksheet.Cell(currentRowCount, 3).Value = "Category";
                    worksheet.Cell(currentRowCount, 4).Value = "Date";
                    worksheet.Cell(currentRowCount, 5).Value = "First Execution Date";
                    worksheet.Cell(currentRowCount, 6).Value = "Execution Period";
                    worksheet.Cell(currentRowCount, 7).Value = "Executor";
                    worksheet.Cell(currentRowCount, 8).Value = "Closing Date";
                    worksheet.Cell(currentRowCount, 9).Value = "Status";

                    foreach (var report in result.Data)
                    {
                        currentRowCount++;
                        worksheet.Cell(currentRowCount, 1).Value = report.RequestId;
                        worksheet.Cell(currentRowCount, 2).Value = report.Creator;
                        worksheet.Cell(currentRowCount, 3).Value = report.Category;
                        worksheet.Cell(currentRowCount, 4).Value = report.CreationDate;
                        worksheet.Cell(currentRowCount, 5).Value = report.ExecutionDate;
                        worksheet.Cell(currentRowCount, 6).Value = report.ExecutionPeriod;
                        worksheet.Cell(currentRowCount, 7).Value = report.Executor;
                        worksheet.Cell(currentRowCount, 8).Value = report.ClosingDate;
                        worksheet.Cell(currentRowCount, 9).Value = report.Status;
                    }

                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var content = stream.ToArray();
                        return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Reports.xlsx");
                    }
                }
            }
            else
            {
                return BadRequest(result);
            }
        }
    }
}
