using AutoMapper;
using Business.Abstract;
using Business.Constants;
using Core.Utilities.Helpers;
using Core.Utilities.Results;
using Core.Utilities.Security.Hashing;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.Dto;
using Entities.Dto.Request;
using Entities.Dto.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Business.Concrete
{
    public class UserManager : IUserService
    {
        private readonly IUserDal _userDal;
        private readonly IMapper _mapper;
        private readonly IFileHelper _fileHelper;
        private readonly IFileDal _fileDal;
        private readonly IConfiguration _configuration;

        public UserManager(IUserDal userDal, IMapper mapper, IFileHelper fileHelper, IConfiguration configuration, IFileDal fileDal)
        {
            _userDal = userDal;
            _mapper = mapper;
            _fileHelper = fileHelper;
            _configuration = configuration;
            _fileDal = fileDal;

        }

        public IDataResult<RespUserDto> GetUserById(int id)
        {
            User user = _userDal.Get(p => p.Id == id).SingleOrDefault();
            RespUserDto respUserDto = _mapper.Map<RespUserDto>(user);

            if (user == null)
            {
                return new ErrorDataResult<RespUserDto>(respUserDto, "User Not found");
            }

            return new SuccessDataResult<RespUserDto>(respUserDto);
        }

        public IDataResult<List<RespUserDto>> GetAllUsers()
        {
            List<User> userList = _userDal.GetAll().ToList();
            List<RespUserDto> respUserDtoList = _mapper.Map<List<RespUserDto>>(userList);
            return new SuccessDataResult<List<RespUserDto>>(respUserDtoList);
        }

        public IResult Add(User user)
        {
            _userDal.Add(user);
            return new SuccessResult();
        }

        public IResult Update(ReqUserDto reqUserDto)
        {
            User updatedUser = _mapper.Map<User>(reqUserDto);
            _userDal.Update(updatedUser);
            return new SuccessResult();
        }

        public IResult Delete(int id)
        {
            User user = _userDal.Get(p => p.Id == id).SingleOrDefault();
            user.Active = false;
            _userDal.Update(user);
            return new SuccessResult();
        }

        public List<OperationClaim> GetClaims(User user)
        {
            return _userDal.GetClaims(user);
        }

        public User GetByUsername(string username)
        {
            var r = _userDal.Get(u => u.Username == username).SingleOrDefault();
            return r;
        }


        public IResult UpdateProfile(UserDto userDto, int userId)
        {
            User user = _userDal.Get(p => p.Id == userId).SingleOrDefault();
            user.Department = userDto.Department;
            user.Position = userDto.Position;
            user.InternalPhone = userDto.InternalPhone;
            user.MobilePhone = userDto.MobilePhone;
            user.NotificationPermit = userDto.NotificationPermit;
            _userDal.Update(user);
            return new SuccessResult();
        }

        public IResult UpdatePassword(int userID, string currentPassword, string newPassword, string confirmNewPassword)
        {
            User user = _userDal.Get(p => p.Id == userID).SingleOrDefault();

            if (!HashingHelper.VerifyPasswordHash(currentPassword, user.PasswordHash, user.PasswordSalt))
            {
                return new ErrorResult(Messages.PasswordNotFound);
            }

            if (newPassword != confirmNewPassword)
            {
                return new ErrorResult(Messages.PasswordIncorrect);
            }

            byte[] passwordHash, passwordSalt;
            HashingHelper.CreatePasswordHash(newPassword, out passwordHash, out passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            _userDal.Update(user);
            return new SuccessResult();
        }

        public IResult UpdateImage(IFormFile file, int userId)
        {
            User user = _userDal.Get(p => p.Id == userId).Include(p => p.File).SingleOrDefault();

            string dir = _configuration["UserImageDir"];
            string path = null;

            if (file != null)
            {
                var extension = Path.GetExtension(file.FileName).ToLower();
                if (!(new[] { ".png", ".jpeg", ".jpg" }.Contains(extension)))
                {
                    return new ErrorResult("The file extension does not match!");
                }

                if (user.File == null)
                {
                    path = _fileHelper.Upload(file, dir);
                }
                else
                {
                    path = _fileHelper.Update(file, dir, user.File.Path);
                    _fileDal.Delete(_fileDal.Get(p => p.Id == user.File.Id).SingleOrDefault());
                }
            }

            Entities.Concrete.File userImageFile = new Entities.Concrete.File()
            {
                Path = path,
                FileName = path.Split("\\").Last(),
                FileOriginalName = file.FileName,
                Extension = Path.GetExtension(file.FileName),
                MimeType = file.ContentType,
                Size = file.Length
            };
            _fileDal.Add(userImageFile);

            user.FileId = userImageFile.Id;
            _userDal.Update(user);

            return new SuccessResult();
        }

        public IResult DeleteImage(int userId)
        {
            User user = _userDal.Get(p => p.Id == userId).Include(p => p.File).SingleOrDefault();

            if (user.File != null)
            {
                _fileHelper.Delete(user.File.Path);

                _fileDal.Delete(user.File);

                user.File = null;
                _userDal.Update(user);
            }

            return new SuccessResult();
        }

       
    }
}
