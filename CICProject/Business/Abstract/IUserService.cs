using Core.Utilities.Results;
using Entities.Concrete;
using Entities.Dto.Request;
using Entities.Dto.Response;

namespace Business.Abstract
{
    public interface IUserService
    {
        IDataResult<List<RespUserDto>> GetAllUsers();
        IDataResult<RespUserDto> GetUserById(int id);
        IResult Add(User userForRegisterDto);
        IResult Update(ReqUserDto user);
        IResult Delete(int id);

        List<OperationClaim> GetClaims(User user);
     //   void Add(User user);
        User GetByUsername(string username);
    }
}
