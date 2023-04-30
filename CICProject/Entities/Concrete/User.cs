using Entities.Abstract;

namespace Entities.Concrete
{
    public class User : IEntity //IdentityUser
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Username { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string Department { get; set; }
        public string Position { get; set; }
        public string InternalPhone { get; set; }
        public string MobilePhone { get; set; }
        public bool NotificationPermit { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool Active { get; set; }


        public ICollection<Request> CreatorRequests { get; set; }
        public ICollection<Request> ExecutorRequests { get; set; }
        public ICollection<UserOperationClaim> UserOperationClaims { get; set; }
        public ICollection<CategoryUser> CategoryUsers { get; set; }=new List<CategoryUser>();

    }
}
