using ANPCentral.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ANPCentral.Data.Mappings
{
    public class AddressMap: IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {

            builder.ToTable
                ("Addresses");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd();

            builder.Property(x => x.Street)
                .IsRequired()
                .HasColumnType("VARCHAR")
                .HasMaxLength(80);


            builder.Property(x => x.City)
                .IsRequired()
                .HasColumnType("VARCHAR")
                .HasMaxLength(80);

            builder.Property(x => x.Complement);

            builder.HasOne(a => a.User)
                .WithOne(u => u.Address)
                .HasForeignKey<Address>("UserId")
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
