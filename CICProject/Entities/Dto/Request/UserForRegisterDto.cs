using Entities.Abstract;
using Microsoft.AspNetCore.Http;

namespace Entities.Dto.Request
{
    public class UserForRegisterDto : IDto
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Mail { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Department { get; set; }
        public string Position { get; set; }
        public string InternalPhone { get; set; }
        public string MobilePhone { get; set; }
        public IFormFile? File { get; set; }
        public List<ReqCategoryUserDto> ReqCategoryUserDto { get; set; }
    }
}
