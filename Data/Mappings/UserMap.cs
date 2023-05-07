using ANPCentral.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ANPCentral.Data.Mappings
{
    public class UserMap: IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {

            builder.ToTable
                ("Users");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd();

            builder.Property(x => x.Name)
                .IsRequired()
                .HasColumnType("NVARCHAR")
                .HasMaxLength(80);


            builder.Property(x => x.Email)
                .IsRequired();

            builder.HasIndex(x => x.Email)
                .IsUnique();
                
            builder.Property(x => x.Password)
                .IsRequired();

        }
    }
}
