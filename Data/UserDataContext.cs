using ANPCentral.Data.Mappings;
using Blog.Models;
using Microsoft.EntityFrameworkCore;

namespace ANPCentral.Data
{
    public class UserDataContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlServer("Server=localhost;Database=ANPCentralDB;User ID=sa;Password=2480;Encrypt=false;");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserMap());
            
        }
    }
}
