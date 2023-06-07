using Core.Utilities.Results;
using Core.Utilities.Security.JWT;
using Entities.Concrete;
using Entities.Dto;
using Entities.Dto.Request;

namespace Business.Abstract
{
    public interface IAuthService
    {
        IDataResult<AccessToken> CreateAccessToken(User user);
        IDataResult<AccessToken> CreateForgetToken(User user);
        IDataResult<User> Register(UserForRegisterDto userForRegisterDto);
        IDataResult<User> Login(UserForLoginDto userForLoginDto);
        IResult ForgotPassword(string mail);
        IResult ResetPassword(ResetPasswordDto resetPassword);
    }
}
