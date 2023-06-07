using Entities.Abstract;

namespace Entities.Concrete
{
    public class NonWorkingDay : IEntity
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
    }
}
