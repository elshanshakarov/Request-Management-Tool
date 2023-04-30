using Entities.Abstract;
using Entities.Concrete;

namespace Entities.Dto.Request
{
    public class UserForRegisterDto : IDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Department { get; set; }
        public string Position { get; set; }
        public string InternalPhone { get; set; }
        public string MobilePhone { get; set; }
        public ReqCategoryUserDto[] ReqCategoryUserDto { get; set; }
    }
}
