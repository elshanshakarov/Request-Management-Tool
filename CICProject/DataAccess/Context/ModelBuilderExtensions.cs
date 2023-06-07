using Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Type = Entities.Concrete.Type;

namespace DataAccess.Context
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RequestType>().HasData(
                new RequestType { Id = 1, Name = "APP Change" },
                new RequestType { Id = 2, Name = "APP Issue" },
                new RequestType { Id = 3, Name = "APP New Requirement" },
                new RequestType { Id = 4, Name = "Change the Report" },
                new RequestType { Id = 5, Name = "Create Custom Report" },
                new RequestType { Id = 6, Name = "Create New Report" },
                new RequestType { Id = 7, Name = "Incident" },
                new RequestType { Id = 8, Name = "Master Data Change" },
                new RequestType { Id = 9, Name = "Service Request" }
                );

            modelBuilder.Entity<Status>().HasData(
                new Status { Id = 1, Name = "Açıq" },
                new Status { Id = 2, Name = "Qapalı" },
                new Status { Id = 3, Name = "İcrada" },
                new Status { Id = 4, Name = "Gözləmədə" },
                new Status { Id = 5, Name = "Təsdiqləndi" },
                new Status { Id = 6, Name = "İmtina edildi" }
                );

            modelBuilder.Entity<Type>().HasData(
                new Type { Id = 1, Name = "Application Maintenance" },
                new Type { Id = 2, Name = "Application Development" }
                );

            modelBuilder.Entity<Priority>().HasData(
                new Priority { Id = 1, Name = "Low" },
                new Priority { Id = 2, Name = "Medium" },
                new Priority { Id = 3, Name = "High" }
                );

            modelBuilder.Entity<Contact>().HasData(
                new Contact { Id = 1, Name = "Email" },
                new Contact { Id = 2, Name = "Phone" },
                new Contact { Id = 3, Name = "Solman" },
                new Contact { Id = 4, Name = "Request" }
                );
        }
    }
}
