using AutoMapper;
using Business.Abstract;
using Business.Constants;
using Core.Utilities.Results;
using Core.Utilities.Security.Hashing;
using Core.Utilities.Security.JWT;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.Dto.Request;

namespace Business.Concrete
{
    public class AuthManager : IAuthService
    {
        private readonly IUserService _userService;
        private readonly ITokenHelper _tokenHelper;
        private readonly IMapper _mapper;
        private readonly ICategoryDal _categoryDal;
        private readonly ICategoryUserDal _categoryUserDal;

        public AuthManager(IUserService userService, ITokenHelper tokenHelper, IMapper mapper, ICategoryDal categoryDal, ICategoryUserDal categoryUserDal)
        {
            _userService = userService;
            _tokenHelper = tokenHelper;
            _mapper = mapper;
            _categoryDal = categoryDal;
            _categoryUserDal = categoryUserDal;
        }

        public IDataResult<User> Register(UserForRegisterDto userForRegisterDto)
        {
            if (_userService.GetByUsername(userForRegisterDto.Username) != null)
            {
                return new ErrorDataResult<User>(null, Messages.UserAlreadyExists);
            }

            byte[] passwordHash, passwordSalt;
            HashingHelper.CreatePasswordHash(userForRegisterDto.Password, out passwordHash, out passwordSalt);

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
                MobilePhone = userForRegisterDto.MobilePhone
            };

            _userService.Add(user);

            foreach (ReqCategoryUserDto reqCategoryUserDto in userForRegisterDto.ReqCategoryUserDto)
            {
                Category category = _categoryDal.Get(c => c.Id == reqCategoryUserDto.CategoryId).SingleOrDefault();
                if (category == null)
                {
                    return new ErrorDataResult<User>(user, Messages.CategoryNotFound);
                }
            }

            foreach (ReqCategoryUserDto reqCategoryUserDto in userForRegisterDto.ReqCategoryUserDto)
            {
                CategoryUser categoryUser = new CategoryUser()
                {
                    Category = new Category { Id = reqCategoryUserDto.CategoryId },
                    User = new User { Id = user.Id },
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

        public IDataResult<AccessToken> CreateAccessToken(User user)
        {
            List<OperationClaim> claims = _userService.GetClaims(user);
            AccessToken accessToken = _tokenHelper.CreateToken(user, claims);
            if (accessToken == null)
            {
                return new ErrorDataResult<AccessToken>(accessToken, Messages.AccessTokenError);
            }
            return new SuccessDataResult<AccessToken>(accessToken, Messages.AccessTokenCreated);
        }
    }
}
