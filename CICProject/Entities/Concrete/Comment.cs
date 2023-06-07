using Entities.Abstract;

namespace Entities.Concrete
{
    public class Comment : IEntity
    {
        public int Id { get; set; }
        public string CommentText { get; set; }
        public int RequestId { get; set; }
        public Request Request { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int? FileId { get; set; }
        public File? File { get; set; }
        public DateTime Date { get; set; }

    }
}
