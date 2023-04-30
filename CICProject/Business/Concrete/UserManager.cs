using AutoMapper;
using Business.Abstract;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.Dto.Request;
using Entities.Dto.Response;

namespace Business.Concrete
{
    public class UserManager : IUserService
    {
        private readonly IUserDal _userDal;
        private readonly IMapper _mapper;

        public UserManager(IUserDal userDal, IMapper mapper)
        {
            _userDal = userDal;
            _mapper = mapper;
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

        //public void Add(User user)
        //{
        //    throw new NotImplementedException();
        //}

        public User GetByUsername(string username)
        {
            var r = _userDal.Get(u => u.Username == username).SingleOrDefault();
            // return _userDal.Get(u => u.Username == username).SingleOrDefault<User>();
            return r;
        }
    }
}
