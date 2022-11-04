using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Task11.Entity.Configurations;

public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
        builder.HasKey(b => b.Id);
        builder.HasIndex(b => b.Login).IsUnique();
        builder.Property(b => b.Login).HasColumnType("nvarchar(25)").IsRequired();
        builder.Property(b => b.Password).HasColumnType("nvarchar(25)").IsRequired();
    }
}
