using Entities.Abstract;

namespace Entities.Concrete
{
    public class CategoryUser : IEntity
    {
        public int Id { get; set; }
        public short CategoryId { get; set; }
        public int UserId { get; set; }   
        public bool CreatePermission { get; set; }
        public bool ExecutePermission { get; set; }

        public Category Category { get; set; }
        public User User { get; set; }
    }
}
