using Entities.Abstract;

namespace Entities.Concrete
{
    public class Contact: IEntity
    {
        public short Id { get; set; }
        public string Name { get; set; }

        public ICollection<Request> Requests { get; set; }
    }
}
