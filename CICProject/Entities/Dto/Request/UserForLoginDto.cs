using Entities.Abstract;

namespace Entities.Dto.Request
{
    public class UserForLoginDto : IDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
