using Microsoft.EntityFrameworkCore;

namespace SharpSeaUsers.Models
{
    public class SharpSeaUsersContext : DbContext
    {
        public SharpSeaUsersContext(DbContextOptions<SharpSeaUsersContext> options) : base (options){}

        public DbSet<User> users { get; set; }
        public DbSet<Conversation> conversations { get; set; }
        public DbSet<Message> messages { get; set; }
    }
}