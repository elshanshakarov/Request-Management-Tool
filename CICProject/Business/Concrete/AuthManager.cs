using AutoMapper;
using Business.Abstract;
using Business.Constants;
using Core.Utilities.Results;
using Core.Utilities.Security.Hashing;
using Core.Utilities.Security.JWT;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.Dto.Request;
using System.Net.Mail;
using System.Net;
using Entities.Dto;
using Core.Utilities.Helpers;

namespace Business.Concrete
{
    public class AuthManager : IAuthService
    {
        private readonly IUserService _userService;
        private readonly IUserDal _userDal;
        private readonly ITokenHelper _tokenHelper;
        private readonly IMapper _mapper;
        private readonly ICategoryDal _categoryDal;
        private readonly ICategoryUserDal _categoryUserDal;

        public AuthManager(IUserService userService, ITokenHelper tokenHelper, IMapper mapper, ICategoryDal categoryDal, ICategoryUserDal categoryUserDal, IUserDal userDal)
        {
            _userService = userService;
            _tokenHelper = tokenHelper;
            _mapper = mapper;
            _categoryDal = categoryDal;
            _categoryUserDal = categoryUserDal;
            _userDal = userDal;
        }

        public IDataResult<AccessToken> CreateAccessToken(User user)
        {
            List<OperationClaim> claims = _userService.GetClaims(user);
            AccessToken accessToken = _tokenHelper.CreateAccessToken(user, claims);
            if (accessToken == null)
            {
                return new ErrorDataResult<AccessToken>(accessToken, Messages.AccessTokenError);
            }
            return new SuccessDataResult<AccessToken>(accessToken, Messages.AccessTokenCreated);
        }

        public IDataResult<AccessToken> CreateForgetToken(User user)
        {
            List<OperationClaim> claims = _userService.GetClaims(user);
            AccessToken accessToken = _tokenHelper.CreateForgetToken(user, claims);
            if (accessToken == null)
            {
                return new ErrorDataResult<AccessToken>(accessToken, Messages.AccessTokenError);
            }
            return new SuccessDataResult<AccessToken>(accessToken, Messages.AccessTokenCreated);
        }

        public IDataResult<User> Register(UserForRegisterDto userForRegisterDto)
        {
            if (_userService.GetByUsername(userForRegisterDto.Username) != null)
            {
                return new ErrorDataResult<User>(Messages.UserAlreadyExists);
            }

            byte[] passwordHash, passwordSalt;
            HashingHelper.CreatePasswordHash(userForRegisterDto.Password, out passwordHash, out passwordSalt);

            //User user =  _mapper.Map<User>(userForRegisterDto);
            User user = new User
            {
                Name = userForRegisterDto.Name,
                Surname = userForRegisterDto.Surname,
                Username = userForRegisterDto.Username,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Department = userForRegisterDto.Department,
                Position = userForRegisterDto.Position,
                InternalPhone = userForRegisterDto.InternalPhone,
                MobilePhone = userForRegisterDto.MobilePhone,
            };
            _userService.Add(user);

            foreach (ReqCategoryUserDto reqCategoryUserDto in userForRegisterDto.ReqCategoryUserDto)
            {
                Category? category = _categoryDal.Get(c => c.Id == reqCategoryUserDto.CategoryId).SingleOrDefault();
                if (category == null)
                {
                    return new ErrorDataResult<User>(user, Messages.CategoryNotFound);
                }

                CategoryUser categoryUser = new CategoryUser()
                {
                    CategoryId = category.Id,
                    UserId = user.Id,
                    CreatePermission = reqCategoryUserDto.CreatePermission,
                    ExecutePermission = reqCategoryUserDto.ExecutePermission
                };
                _categoryUserDal.Add(categoryUser);
            }

            return new SuccessDataResult<User>(user, Messages.UserRegistered);
        }

        public IDataResult<User> Login(UserForLoginDto userForLoginDto)
        {
            User userToCheck = _userService.GetByUsername(userForLoginDto.Username);
            if (userToCheck == null)
            {
                return new ErrorDataResult<User>(userToCheck, Messages.UsernameNotFound);
            }

            if (!HashingHelper.VerifyPasswordHash(userForLoginDto.Password, userToCheck.PasswordHash, userToCheck.PasswordSalt))
            {
                return new ErrorDataResult<User>(userToCheck, Messages.PasswordNotFound);
            }

            return new SuccessDataResult<User>(userToCheck, Messages.SuccessfulLogin);
        }

        public IResult ForgotPassword(string mail)
        {
            User? user = _userDal.Get(p => p.Mail == mail).SingleOrDefault();
            if (user == null)
            {
                return new ErrorResult(Messages.UserNotFound);
            }
            string token = CreateForgetToken(user).Data.Token;
            string link = "https://localhost:44365/api/Auth/ResetPassword?token=" + token;

            MailHelper.Send(mail, "from@example.com", "Forgot Password", link);

            return new SuccessResult("Mail has been sent. Please open your email and click the link.");
        }

        public IResult ResetPassword(ResetPasswordDto resetPassword)
        {
            string email;
            var isTrue = JwtHelper.VerifyResetToken(resetPassword.Token, out email);

            if (isTrue && resetPassword.Password == resetPassword.ConfirmPassword)
            {
                var user = _userDal.Get(p => p.Mail == email).SingleOrDefault();
                byte[] passwordHash, passwordSalt;
                HashingHelper.CreatePasswordHash(resetPassword.Password, out passwordHash, out passwordSalt);
                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
                _userDal.Update(user);
                return new SuccessResult("The password reset successfully");
            }
            return new ErrorResult("The password didn't reset successfully");
        }
    }
}
