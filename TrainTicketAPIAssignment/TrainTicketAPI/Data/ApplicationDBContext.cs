using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TrainTicketAPI.Models;

namespace TrainTicketAPI.Data
{
    public class ApplicationDBContext : IdentityDbContext<User>
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
        }

        public DbSet<Train> Train { get; set; }

        public DbSet<Ticket> Ticket { get; set; }

        public DbSet<Schedule> Schedule { get; set; }
        
    }
}
