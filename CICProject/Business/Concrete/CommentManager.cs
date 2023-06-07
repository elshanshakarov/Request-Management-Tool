using AutoMapper;
using Business.Abstract;
using Core.Utilities.Helpers;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.Dto;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Business.Concrete
{
    public class CommentManager : ICommentService
    {
        private readonly ICommentDal _commentDal;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IFileDal _fileDal;
        private readonly IFileHelper _fileHelper;
        public CommentManager(ICommentDal commentDal, IMapper mapper, IConfiguration configuration, IFileDal fileDal, IFileHelper fileHelper)
        {
            _commentDal = commentDal;
            _mapper = mapper;
            _configuration = configuration;
            _fileDal = fileDal;
            _fileHelper = fileHelper;
        }

        public IDataResult<List<RespCommentDto>> GetAllCommentsByRequestId(int userId, int requestId)
        {
            List<Comment> commentList = _commentDal.GetAll(p => p.RequestId == requestId).Include(c => c.User).ToList();
            List<RespCommentDto> commentDtosList = _mapper.Map<List<RespCommentDto>>(commentList);

            return new SuccessDataResult<List<RespCommentDto>>(commentDtosList);
        }

        public IResult AddComment(ReqCommentDto reqComment, int userId)
        {
            string path = null;
            string dir = _configuration["RequestCommentFileDir"];

            if (reqComment.File != null)
            {
                var extension = Path.GetExtension(reqComment.File.FileName).ToLower();

                if (!(new[] { ".png", ".jpeg", ".jpg", ".pdf" }.Contains(extension)))
                {
                    return new ErrorResult("The file extension does not match!");
                }

                path = _fileHelper.Upload(reqComment.File, dir);
            }

            Entities.Concrete.File reqCommentFile = new Entities.Concrete.File()
            {
                Path = path,
                FileName = path.Split("\\").Last(),
                FileOriginalName = reqComment.File.FileName,
                Extension = Path.GetExtension(reqComment.File.FileName),
                MimeType = reqComment.File.ContentType,
                Size = reqComment.File.Length
            };
            _fileDal.Add(reqCommentFile);

            Comment comment = new Comment
            {
                CommentText = reqComment.CommentText,
                Date = DateTime.Now,
                RequestId = reqComment.RequestId,
                UserId = userId,
                FileId = reqCommentFile.Id
            };
            _commentDal.Add(comment);

            return new SuccessResult();
        }
    }
}
