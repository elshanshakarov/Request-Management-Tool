using Entities.Abstract;

namespace Entities.Concrete
{
    public class History : IEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int RequestId { get; set; }
        public Request Request { get; set; }
        public string Message { get; set; }
        public DateTime Date { get; set; }
    }
}
