using AutoMapper;
using Business.Abstract;
using Business.Constants;
using Core.Utilities.Helpers;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.Dto;
using Entities.Dto.Request;
using Entities.Dto.Response;
using Entities.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using File = Entities.Concrete.File;

namespace Business.Concrete
{
    public class RequestManager : IRequestService
    {
        private readonly IRequestDal _requestDal;
        private readonly ICategoryUserDal _categoryUserDal;
        private readonly ICommentDal _commentDal;
        private readonly IHistoryDal _historyDal;
        private readonly INonWorkingDayDal _nonWorkingDayDal;
        public readonly IFileDal _fileDal;
        public readonly IFileHelper _fileHelper;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IUserDal _userDal;


        public RequestManager(IRequestDal requestDal, IMapper mapper, ICategoryUserDal categoryUserDal, ICommentDal commentDal, IHistoryDal historyDal, INonWorkingDayDal nonWorkingDayDal, IConfiguration configuration, IFileDal fileDal, IFileHelper fileHelper, IUserDal userDal)
        {
            _requestDal = requestDal;
            _mapper = mapper;
            _categoryUserDal = categoryUserDal;
            _commentDal = commentDal;
            _historyDal = historyDal;
            _nonWorkingDayDal = nonWorkingDayDal;
            _configuration = configuration;
            _fileDal = fileDal;
            _fileHelper = fileHelper;
            _userDal = userDal;
        }

        public IResult AddRequest(ReqRequestDto requestDto, int userId)
        {
            CategoryUser? createPermissionCategory = _categoryUserDal.GetAll(p => p.User.Id == userId && p.CreatePermission == true && p.CategoryId == requestDto.CategoryId).SingleOrDefault();
            if (createPermissionCategory == null)
            {
                return new ErrorResult(Messages.IncorrectCategory);
            }

            string dir = _configuration["RequestFileDir"];
            string path = null;

            File? requestFile = null;
            if (requestDto.File != null)
            {
                var extension = Path.GetExtension(requestDto.File.FileName).ToLower();
                if (!(new[] { ".png", ".jpeg", ".jpg", ".pdf" }.Contains(extension)))
                {
                    return new ErrorResult("The file extension does not match!");
                }
                path = _fileHelper.Upload(requestDto.File, dir);

                requestFile.Path = path;
                requestFile.FileName = path.Split("\\").Last();
                requestFile.FileOriginalName = requestDto.File.FileName;
                requestFile.Extension = Path.GetExtension(requestDto.File.FileName);
                requestFile.MimeType = requestDto.File.ContentType;
                requestFile.Size = requestDto.File.Length;
                _fileDal.Add(requestFile);
            }

            Request request = new Request
            {
                CreatorId = userId,
                CategoryId = requestDto.CategoryId,
                PriorityId = requestDto.PriorityId,
                RequestTypeId = requestDto.RequestTypeId,
                Tittle = requestDto.Tittle,
                Text = requestDto.Text,
                FileId = requestFile == null ? null : requestFile.Id
            };
            _requestDal.Add(request);

            History history = new History() { RequestId = request.Id, UserId = request.CreatorId, Date = DateTime.Now, Message = Messages.RequestCreated };
            _historyDal.Add(history);

            var cu = _categoryUserDal.GetAll(p => p.CategoryId == request.CategoryId && p.ExecutePermission == true && p.UserId != request.CreatorId).Include(u => u.User).ToList();
            foreach (var c in cu)
            {
                MailHelper.Send(c.User.Mail, "Admin@cic.com", "Request Message", $"{request.Id} nömrəli sorğu yaradıldı. Siz bu sorğunu icra edə bilərsiz.");
            }

            return new SuccessResult();
        }

        public IResult UpdateRequest(RequestInfoDto requestInfoDto, int userId)
        {
            Request? request = _requestDal.Get(p => p.Id == requestInfoDto.RequestId).Where(p => p.ExecutorId == userId).SingleOrDefault();
            if (request == null)
            {
                return new ErrorResult(Messages.RequestNotFound);
            }
            _mapper.Map(requestInfoDto, request);
            _requestDal.Update(request);
            return new SuccessResult();
        }

        public IDataResult<List<RespRequestDto>> GetAllRequests(int userId, AllRequestFilterDto allRequestFilterDto)
        {
            List<Request> requestList = _requestDal.GetAllFilteredRequests(userId, allRequestFilterDto)
               .Include(c => c.Creator)
               .Include(c => c.Category)
               .Include(c => c.Executor)
               .Include(c => c.Status)
               .Select(r => r)
               .ToList();

            if (requestList.Count == 0)
            {
                return new ErrorDataResult<List<RespRequestDto>>("Request not found!");
            }

            foreach (var filter in allRequestFilterDto.Filter)
            {
                string search = filter.Search.ToLower();
                string value = filter.Value.ToLower();
                switch (search)
                {
                    case "id":
                        requestList = requestList.Where(p => p.Id.ToString().Contains(value)).ToList();
                        break;
                    case "creator":
                        requestList = requestList.Where(p => p.Creator.Username.ToLower().Contains(value)).ToList();
                        break;
                    case "tittle":
                        requestList = requestList.Where(p => p.Tittle.Contains(value)).ToList();
                        break;
                    case "text":
                        requestList = requestList.Where(p => p.Text.Contains(value)).ToList();
                        break;
                    case "category":
                        requestList = requestList.Where(p => p.Category.Name.ToLower().Contains(value)).ToList();
                        break;
                    case "executor":
                        requestList = requestList.Where(p => p.Executor.Name.ToLower().Contains(value)).ToList();
                        break;
                    case "date":
                        requestList = requestList.Where(p => Convert.ToString(p.Date).Contains(value)).ToList();
                        break;
                    case "status":
                        requestList = requestList.Where(p => p.Status.Name.ToLower().Contains(value)).ToList();
                        break;
                    default:
                        break;
                }
            }
            //var totalSize = requestList.Count;
            //var totalPages = (int)Math.Ceiling((decimal)totalSize / requestFilterDto.PageSize);

            List<RespRequestDto> requestDtoList = _mapper.Map<List<RespRequestDto>>(requestList)
                .Skip((allRequestFilterDto.Page - 1) * allRequestFilterDto.PageSize)
                .Take(allRequestFilterDto.PageSize)
                .ToList();

            return new SuccessDataResult<List<RespRequestDto>>(requestDtoList);
        }

        public IDataResult<List<RespRequestDto>> GetAllMyRequests(int userId, MyRequestFilterDto myRequestFilterDto)
        {
            List<Request> requestList = _requestDal.GetAllMyRequests(userId, myRequestFilterDto.StatusId)
                .Include(c => c.Creator)
                .Include(c => c.Category)
                .Include(c => c.Executor)
                .Include(c => c.Status)
                .ToList();

            if (requestList.Count == 0)
            {
                return new ErrorDataResult<List<RespRequestDto>>("Requests not found!");
            }

            foreach (var filter in myRequestFilterDto.Filter)
            {
                string search = filter.Search.ToLower();
                string value = filter.Value.ToLower();
                switch (search)
                {
                    case "id":
                        requestList = requestList.Where(p => p.Id.ToString().Contains(value)).ToList();
                        break;
                    case "creator":
                        requestList = requestList.Where(p => p.Creator.Username.ToLower().Contains(value)).ToList();
                        break;
                    case "tittle":
                        requestList = requestList.Where(p => p.Tittle.Contains(value)).ToList();
                        break;
                    case "text":
                        requestList = requestList.Where(p => p.Text.Contains(value)).ToList();
                        break;
                    case "category":
                        requestList = requestList.Where(p => p.Category.Name.ToLower().Contains(value)).ToList();
                        break;
                    case "executor":
                        requestList = requestList.Where(p => p.Executor.Name.ToLower().Contains(value)).ToList();
                        break;
                    case "date":
                        requestList = requestList.Where(p => Convert.ToString(p.Date).Contains(value)).ToList();
                        break;
                    case "status":
                        requestList = requestList.Where(p => p.Status.Name.ToLower().Contains(value)).ToList();
                        break;
                    default:
                        break;
                }
            }
            //var totalSize = requestList.Count;
            //var totalPages = (int)Math.Ceiling((decimal)totalSize / pageSize);

            List<RespRequestDto> respRequests = _mapper.Map<List<RespRequestDto>>(requestList)
                .Skip((myRequestFilterDto.Page - 1) * myRequestFilterDto.PageSize)
                .Take(myRequestFilterDto.PageSize)
                .ToList();

            return new SuccessDataResult<List<RespRequestDto>>(respRequests);
        }

        public IDataResult<List<CountOfRequestsDto>> GetCountOfAllMyRequests(int userId, short? statusId)
        {
            List<CountOfRequestsDto> result = _requestDal.GetCountOfAllMyRequests(userId, statusId);

            return new SuccessDataResult<List<CountOfRequestsDto>>(result);
        }

        public IDataResult<List<CountOfRequestsDto>> GetCountOfAllRequests(int userId, AllRequestFilterDto requestFilterDto)
        {
            List<CountOfRequestsDto> result = _requestDal.GetCountOfAllRequests(userId, requestFilterDto);

            return new SuccessDataResult<List<CountOfRequestsDto>>(result);
        }

        public IDataResult<RequestDto> GetRequestById(int userId, int requestId)
        {
            Request? request = _requestDal.GetRequestById(userId, requestId)
                        .Include(c => c.Creator)
                        .Include(c => c.Status)
                        .Include(c => c.RequestType)
                        .Include(c => c.Priority)
                        .SingleOrDefault();
            if (request == null)
            {
                return new ErrorDataResult<RequestDto>(Messages.RequestNotFound);
            }

            RespCommentDto commentDto = new RespCommentDto();
            if (_commentDal.GetAll().Any(c => c.RequestId == requestId))
            {
                Comment? lastComment = _commentDal.GetAll(p => p.RequestId.Equals(requestId)).Include(c => c.User).ToList().Last();
                commentDto = _mapper.Map(lastComment, commentDto);
            }
            else
            {
                commentDto = null;
            }

            RequestDto requestByIdRequestDto = new RequestDto();
            requestByIdRequestDto = _mapper.Map(request, requestByIdRequestDto);
            requestByIdRequestDto.LastComment = commentDto;

            return new SuccessDataResult<RequestDto>(requestByIdRequestDto);
        }

        public IDataResult<List<ReportDto>> GetAllReports(ReportFilterDto reportFilterDto, int userId)
        {
            List<ReportDto> reportList = new List<ReportDto>();
            List<Request> requestList = _requestDal.GetAll(p => (p.CreatorId == userId || p.ExecutorId == userId) && (p.StatusId == (short)AvailableStatus.Close || p.StatusId == (short)AvailableStatus.Confirm))
                .Include(p => p.Creator)
                .Include(p => p.Category)
                .Include(p => p.Executor)
                .Include(p => p.Status)
                .ToList();
            foreach (var request in requestList)
            {
                var history = _historyDal.GetAll(p => p.Request.Id == request.Id);

                DateTime executionDate = _historyDal.GetAll(p => p.Request.Id == request.Id && p.Message == Messages.LockMessage).Select(p => p.Date).First();
                DateTime closingDate = _historyDal.GetAll(p => p.Request.Id == request.Id && p.Message == Messages.CloseMessage).Select(p => p.Date).First();
                TimeSpan subtraction = (closingDate - executionDate);
                int nonWorkingDayCount = _nonWorkingDayDal.GetAll(p => executionDate < p.Date && p.Date < closingDate).Count();
                double executionPeriod = subtraction.TotalHours - (subtraction.Days * (24 - 9)) - nonWorkingDayCount * 9;

                ReportDto reportDto = new ReportDto
                {
                    RequestId = request.Id,
                    Creator = request.Creator.Username,
                    Category = request.Category.Name,
                    CreationDate = _historyDal.Get(p => p.RequestId == request.Id && p.Message == Messages.RequestCreated).First().Date.ToString("dd.MM.yyyy"),
                    ExecutionDate = _historyDal.Get(p => p.RequestId == request.Id && p.Message == Messages.LockMessage).First().Date.ToString("dd.MM.yyyy"),
                    ExecutionPeriod = Math.Round(executionPeriod, 2),
                    Executor = request.Executor.Username,
                    ClosingDate = _historyDal.Get(p => p.RequestId == request.Id && p.Message == Messages.CloseMessage).First().Date.ToString("dd.MM.yyyy"),
                    Status = request.Status.Name
                };

                reportList.Add(reportDto);
            }

            reportList = reportList.Where(p => DateTime.Parse(p.CreationDate) >= DateTime.Parse(reportFilterDto.StartDateRange) && DateTime.Parse(p.CreationDate) <= DateTime.Parse(reportFilterDto.EndDateRange)).Skip((reportFilterDto.Page - 1) * reportFilterDto.PageSize).Take(reportFilterDto.PageSize).ToList();

            foreach (var filter in reportFilterDto.Filter)
            {
                string search = filter.Search.ToLower();
                string value = filter.Value.ToLower();
                switch (search)
                {
                    case "id":
                        reportList = reportList.Where(p => p.RequestId.ToString().Contains(value)).ToList();
                        break;
                    case "creator":
                        reportList = reportList.Where(p => p.Creator.ToLower().Contains(value.ToLower())).ToList();
                        break;
                    case "category":
                        reportList = reportList.Where(p => p.Category.ToLower().Contains(value.ToLower())).ToList();
                        break;
                    case "creation date":
                        reportList = reportList.Where(p => p.CreationDate.Contains(value)).ToList();
                        break;
                    case "execution date":
                        reportList = reportList.Where(p => p.ExecutionDate.Contains(value)).ToList();
                        break;
                    case "execution period":
                        reportList = reportList.Where(p => p.ExecutionPeriod.ToString().Contains(value)).ToList();
                        break;
                    case "executor":
                        reportList = reportList.Where(p => p.Executor.ToLower().Contains(value.ToLower())).ToList();
                        break;
                    case "closing date":
                        reportList = reportList.Where(p => p.ClosingDate.Contains(value)).ToList();
                        break;
                    case "status":
                        reportList = reportList.Where(p => p.Status.ToLower().Contains(value.ToLower())).ToList();
                        break;
                    default:
                        break;
                }
            }
            return new SuccessDataResult<List<ReportDto>>(reportList);
        }

        public IDataResult<List<ReportDto>> GetAllExcelReports(ExcelFilterDto excelFilterDto, int userId)
        {
            List<ReportDto> reportList = new List<ReportDto>();
            List<Request> requestList = _requestDal.GetAll(p => (p.CreatorId == userId || p.ExecutorId == userId) && (p.StatusId == (short)AvailableStatus.Close || p.StatusId == (short)AvailableStatus.Confirm))
                .Include(p => p.Creator)
                .Include(p => p.Category)
                .Include(p => p.Executor)
                .Include(p => p.Status)
                .ToList();
            foreach (var request in requestList)
            {
                var history = _historyDal.GetAll(p => p.Request.Id == request.Id);

                DateTime executionDate = _historyDal.GetAll(p => p.Request.Id == request.Id && p.Message == Messages.LockMessage).Select(p => p.Date).First();
                DateTime closingDate = _historyDal.GetAll(p => p.Request.Id == request.Id && p.Message == Messages.CloseMessage).Select(p => p.Date).First();
                TimeSpan subtraction = (closingDate - executionDate);
                int nonWorkingDayCount = _nonWorkingDayDal.GetAll(p => executionDate < p.Date && p.Date < closingDate).Count();
                double executionPeriod = subtraction.TotalHours - (subtraction.Days * (24 - 9)) - nonWorkingDayCount * 9;

                ReportDto reportDto = new ReportDto
                {
                    RequestId = request.Id,
                    Creator = request.Creator.Username,
                    Category = request.Category.Name,
                    CreationDate = _historyDal.Get(p => p.RequestId == request.Id && p.Message == Messages.RequestCreated).First().Date.ToString("dd.MM.yyyy"),
                    ExecutionDate = _historyDal.Get(p => p.RequestId == request.Id && p.Message == Messages.LockMessage).First().Date.ToString("dd.MM.yyyy"),
                    ExecutionPeriod = Math.Round(executionPeriod, 2),
                    Executor = request.Executor.Username,
                    ClosingDate = _historyDal.Get(p => p.RequestId == request.Id && p.Message == Messages.CloseMessage).First().Date.ToString("dd.MM.yyyy"),
                    Status = request.Status.Name
                };

                reportList.Add(reportDto);
            }

            reportList = reportList.Where(p => DateTime.Parse(p.CreationDate) >= DateTime.Parse(excelFilterDto.StartDateRange) && DateTime.Parse(p.CreationDate) <= DateTime.Parse(excelFilterDto.EndDateRange)).ToList();

            foreach (var filter in excelFilterDto.Filter)
            {
                string search = filter.Search.ToLower();
                string value = filter.Value.ToLower();
                switch (search)
                {
                    case "id":
                        reportList = reportList.Where(p => p.RequestId.ToString().Contains(value)).ToList();
                        break;
                    case "creator":
                        reportList = reportList.Where(p => p.Creator.ToLower().Contains(value.ToLower())).ToList();
                        break;
                    case "category":
                        reportList = reportList.Where(p => p.Category.ToLower().Contains(value.ToLower())).ToList();
                        break;
                    case "creation date":
                        reportList = reportList.Where(p => p.CreationDate.Contains(value)).ToList();
                        break;
                    case "execution date":
                        reportList = reportList.Where(p => p.ExecutionDate.Contains(value)).ToList();
                        break;
                    case "execution period":
                        reportList = reportList.Where(p => p.ExecutionPeriod.ToString().Contains(value)).ToList();
                        break;
                    case "executor":
                        reportList = reportList.Where(p => p.Executor.ToLower().Contains(value.ToLower())).ToList();
                        break;
                    case "closing date":
                        reportList = reportList.Where(p => p.ClosingDate.Contains(value)).ToList();
                        break;
                    case "status":
                        reportList = reportList.Where(p => p.Status.ToLower().Contains(value.ToLower())).ToList();
                        break;
                    default:
                        break;
                }
            }
            return new SuccessDataResult<List<ReportDto>>(reportList);
        }
    }
}
