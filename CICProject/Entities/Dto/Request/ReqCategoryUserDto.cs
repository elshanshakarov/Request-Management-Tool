namespace Entities.Dto.Request
{
    public class ReqCategoryUserDto
    {
        public short CategoryId { get; set; }
        public bool CreatePermission { get; set; }
        public bool ExecutePermission { get; set; }
    }
}
