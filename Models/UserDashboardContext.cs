using Microsoft.EntityFrameworkCore;

namespace UserDashboard.Models
{
    public class UserDashboardContext : DbContext
    {
        public UserDashboardContext(DbContextOptions<UserDashboardContext> options) : base (options){}

        public DbSet<User> users { get; set; }
        public DbSet<Conversation> conversations { get; set; }
        public DbSet<Message> messages { get; set; }
    }
}