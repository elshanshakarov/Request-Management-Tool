using Entities.Abstract;

namespace Entities.Concrete
{
    public class File : IEntity
    {
        public int? Id { get; set; }
        public string Path { get; set; }
        public string FileName { get; set; }
        public string FileOriginalName { get; set; }
        public string Extension { get; set; }
        public string MimeType { get; set; }
        public long Size { get; set; }
        public Request Request { get; set; }
        public Comment Comment { get; set; }
        public User User { get; set; }
    }
}
