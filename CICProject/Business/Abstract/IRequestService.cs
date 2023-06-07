using Core.Utilities.Results;
using Entities.Dto;
using Entities.Dto.Request;
using Entities.Dto.Response;

namespace Business.Abstract
{
    public interface IRequestService
    {
        IResult AddRequest(ReqRequestDto requestDto, int userId);
        IResult UpdateRequest(RequestInfoDto requestInfoDto, int userId);
        IDataResult<List<RespRequestDto>> GetAllRequests(int userId, AllRequestFilterDto requestFilterDto);
        IDataResult<List<RespRequestDto>> GetAllMyRequests(int userId, MyRequestFilterDto myRequestFilterDto);
        IDataResult<List<CountOfRequestsDto>> GetCountOfAllMyRequests(int userId, short? statusId);
        IDataResult<List<CountOfRequestsDto>> GetCountOfAllRequests(int userId, AllRequestFilterDto requestFilterDto);
        IDataResult<RequestDto> GetRequestById(int userId, int requestId);
        IDataResult<List<ReportDto>> GetAllReports(ReportFilterDto reportFilterDto, int userId);
        IDataResult<List<ReportDto>> GetAllExcelReports(ExcelFilterDto excelFilterDto, int userId);
    }
}
