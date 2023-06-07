using Entities.Abstract;

namespace Entities.Concrete
{
    public class Request : IEntity
    {
        public int Id { get; set; }
        public int CreatorId { get; set; }
        public User Creator { get; set; }
        public int? ExecutorId { get; set; }
        public User? Executor { get; set; }
        public short CategoryId { get; set; }
        public Category Category { get; set; }
        public short PriorityId { get; set; }
        public Priority Priority { get; set; }
        public short RequestTypeId { get; set; }
        public RequestType RequestType { get; set; }
        public string Tittle { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public short StatusId { get; set; }
        public Status Status { get; set; }
        public string? Result { get; set; }
        public string? Solution { get; set; }
        public double? ExecutionTime { get; set; }
        public double? PlannedExecutionTime { get; set; }
        public short? TypeId { get; set; }
        public Type? Type { get; set; }
        public string? RequestSender { get; set; }
        public string? SolmanRequestNumber { get; set; }
        public short? ContactId { get; set; }
        public Contact? Contact { get; set; }
        public bool? Rountine { get; set; }
        public string? Code { get; set; }
        public string? RootCause { get; set; }
        public int? FileId { get; set; }
        public File? File { get; set; }

        public ICollection<History> Histories { get; set; }
        public ICollection<Comment> Comments { get; set; }

    }
}
