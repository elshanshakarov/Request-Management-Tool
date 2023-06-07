namespace Entities.Dto
{
    public class UserDto
    {
        public string Department { get; set; }
        public string Position { get; set; }
        public string InternalPhone { get; set; }
        public string MobilePhone { get; set; }
        public bool NotificationPermit { get; set; } = true;
    }
}
