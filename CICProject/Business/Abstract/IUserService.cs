using Core.Utilities.Results;
using Entities.Concrete;
using Entities.Dto;
using Entities.Dto.Request;
using Entities.Dto.Response;
using Microsoft.AspNetCore.Http;

namespace Business.Abstract
{
    public interface IUserService
    {
        IDataResult<List<RespUserDto>> GetAllUsers();
        IDataResult<RespUserDto> GetUserById(int id);
        IResult Add(User userForRegisterDto);
        IResult Update(ReqUserDto user);
        IResult Delete(int id);

        IResult UpdateProfile(UserDto userDto, int userId);
        IResult UpdatePassword(int userId, string currentPassword, string newPassword, string againNewPassword);
        IResult UpdateImage(IFormFile file, int userId);
        IResult DeleteImage(int userId);

       


        List<OperationClaim> GetClaims(User user);
        User GetByUsername(string username);
    }
}
