using AutoMapper;
using Business.Abstract;
using Business.Constants;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.Dto;
using Entities.Dto.Request;
using Entities.Dto.Response;
using Microsoft.EntityFrameworkCore;

namespace Business.Concrete
{
    public class RequestManager : IRequestService
    {
        private readonly IRequestDal _requestDal;
        private readonly IMapper _mapper;
        private readonly ICategoryDal _categoryDal;
        private readonly ICategoryUserDal _categoryUserDal;
        private readonly ICommentDal _commentDal;
        private readonly IHistoryDal _historyDal;

        public RequestManager(IRequestDal requestDal, IMapper mapper, ICategoryDal categoryDal, ICategoryUserDal categoryUserDal, ICommentDal commentDal, IHistoryDal historyDal)
        {
            _requestDal = requestDal;
            _mapper = mapper;
            _categoryDal = categoryDal;
            _categoryUserDal = categoryUserDal;
            _commentDal = commentDal;
            _historyDal = historyDal;
        }

        public IResult Add(Request request)
        {
            CategoryUser createPermissionCategory = _categoryUserDal.GetAll(p => p.User.Id.Equals(request.CreatorId) && p.CreatePermission.Equals(true) && p.CategoryId.Equals(request.CategoryId)).SingleOrDefault();

            if (createPermissionCategory == null)
            {
                return new ErrorResult(Messages.IncorrectCategory);
            }
            _requestDal.Add(request);

            History history = new History() { RequestId = request.Id, UserId = request.CreatorId, Date = DateTime.Now, Message = Messages.RequestCreated };
            _historyDal.Add(history);

            return new SuccessResult();
        }

        public IResult Delete(int id)
        {
            Request? request = _requestDal.Get(r => r.Id == id).SingleOrDefault();
            _requestDal.Delete(request);
            return new SuccessResult();
        }

        public IResult Update(ReqRequestDto reqRequestDto)
        {
            Request request = _mapper.Map<Request>(reqRequestDto);
            _requestDal.Update(request);
            return new SuccessResult();
        }


        public IDataResult<List<RespRequestDto>> GetAllFilteredRequests(int userId, RequestFilterDto requestFilterDto)
        {
            List<Request> requestList = _requestDal.GetAllFilteredRequests(userId, requestFilterDto)
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

            List<RespRequestDto> requestDtoList = _mapper.Map<List<RespRequestDto>>(requestList);

            return new SuccessDataResult<List<RespRequestDto>>(requestDtoList);
        }

        public IDataResult<List<RespRequestDto>> GetAllMyRequests(int userId, short? statusId)
        {
            List<Request> requestList = _requestDal.GetAllMyRequests(userId, statusId)
                .Include(c => c.Creator)
                .Include(c => c.Category)
                .Include(c => c.Executor)
                .Include(c => c.Status)
                .ToList();

            if (requestList.Count == 0)
            {
                return new ErrorDataResult<List<RespRequestDto>>("Requests not found!");
            }

            List<RespRequestDto> respRequests = _mapper.Map<List<RespRequestDto>>(requestList);

            return new SuccessDataResult<List<RespRequestDto>>(respRequests);
        }

        public IDataResult<List<CountOfRequestsDto>> GetCountOfAllMyRequests(int userId, short? statusId)
        {
            List<CountOfRequestsDto> result = _requestDal.GetCountOfAllMyRequests(userId, statusId);

            return new SuccessDataResult<List<CountOfRequestsDto>>(result);
        }

        public IDataResult<List<CountOfRequestsDto>> GetCountOfAllRequests(int userId, RequestFilterDto requestFilterDto)
        {
            List<CountOfRequestsDto> result = _requestDal.GetCountOfAllRequests(userId, requestFilterDto);

            return new SuccessDataResult<List<CountOfRequestsDto>>(result);
        }

        public IDataResult<RequestByIdRequestDto> GetRequestByIdRequestDto(int userId, int requestId)
        {
            Request? request = _requestDal.GetExecuteRequestById(userId, requestId)
                        .Include(c => c.Creator)
                        .Include(c => c.Category)
                        .Include(c => c.Executor)
                        .Include(c => c.Status)
                        .Include(c => c.RequestType)
                        .Include(c => c.Priority)
                        .SingleOrDefault();
            if (request == null)
            {
                return new ErrorDataResult<RequestByIdRequestDto>("Request not found");
            }

            CommentDto commentDto = new CommentDto();
            if (_commentDal.GetAll().Any(c => c.RequestId == requestId))
            {
                Comment? lastComment = _commentDal.GetAll(p => p.RequestId.Equals(requestId)).Include(c => c.User).ToList().Last();
                commentDto.FullName = lastComment.User.Name + " " + lastComment.User.Surname;
                commentDto.Position = lastComment.User.Position;
                commentDto.Department = lastComment.User.Department;
                commentDto.CommentDate = lastComment.Date;
                commentDto.Comment = lastComment.CommentText;
            }
            else
            {
                commentDto = null;
            }

            RequestByIdRequestDto requestByIdRequestDto = new RequestByIdRequestDto()
            {
                Status = request.Status.Name,
                CreatorFullName = request.Creator.Name + " " + request.Creator.Surname,
                CreatorPosition = request.Creator.Position,
                CreatorDepartment = request.Creator.Department,
                CreatorCommentDate = request.Date,
                CreatorComment = request.Text,
                RequestType = request.RequestType.Name,
                Priority = request.Priority.Name,
                LastComment = commentDto
            };

            return new SuccessDataResult<RequestByIdRequestDto>(requestByIdRequestDto);
        }

        public IResult ChangeStatus(int requestId, int userId, short statusId)
        {
            Request request = _requestDal.Get(p => p.Id.Equals(requestId)).SingleOrDefault();

            if (request.CreatorId == userId)
            {
                if (request.StatusId == 2 && statusId != 5)
                {
                    return new ErrorResult();
                }
                else if (request.StatusId != 2 && statusId != null)
                {
                    return new ErrorResult();
                }
            }
            else if (request.CreatorId != userId)
            {
                if (request.StatusId == 1 && (statusId != 3 || statusId != 6))
                {
                    return new ErrorResult();
                }
                else if (request.StatusId == 2 && statusId != null)
                {
                    return new ErrorResult();
                }
                else if (request.StatusId == 3)
                {
                    if (request.ExecutorId == userId && (statusId != 2 || statusId != 4))
                    {
                        return new ErrorResult();
                    }
                    else if (request.ExecutorId != userId && statusId != null)
                    {
                        return new ErrorResult();
                    }
                }
                else if (request.StatusId == 4)
                {
                    if (request.ExecutorId == userId && statusId != 3)
                    {
                        return new ErrorResult();
                    }
                    else if (request.ExecutorId != userId && statusId != null)
                    {
                        return new ErrorResult();
                    }
                }
                else if ((request.StatusId == 5 || request.StatusId == 6) && statusId != null)
                {
                    return new ErrorResult();
                }
            }

            return null;
        }

        public IDataResult<List<CommentDto>> GetRequestByIdCommentDto(int userId, int requestId)
        {
            List<Comment> commentList = _commentDal.GetAll().Include(c => c.User).ToList();
            List<CommentDto> commentDtosList = new List<CommentDto>();

            foreach (var comment in commentList)
            {
                CommentDto commentDto = new CommentDto()
                {
                    FullName = comment.User.Name + " " + comment.User.Surname,
                    Position = comment.User.Position,
                    Department = comment.User.Department,
                    CommentDate = comment.Date,
                    Comment = comment.CommentText
                };
                commentDtosList.Add(commentDto);
            }

            return new SuccessDataResult<List<CommentDto>>(commentDtosList);
        }

        public IResult AddComment(int userId, int requestId, string commentText)
        {
            Comment comment = new Comment { CommentText = commentText, Date = DateTime.Now, RequestId = requestId, UserId = userId };
            _commentDal.Add(comment);
            return new SuccessResult();
        }


    }
}
