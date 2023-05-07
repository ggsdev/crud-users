using ANPCentral.Data.Mappings;
using ANPCentral.Models;
using Microsoft.EntityFrameworkCore;

namespace ANPCentral.Data
{
    public class UserDataContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlServer("Server=localhost;Database=ANPCentralDB;User ID=garcia;Password=2480;Encrypt=false;Trusted_Connection=True;");



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserMap());
            
        }
    }
}
