using Domain.Images;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Images;

internal sealed class UserImageConfiguration : IEntityTypeConfiguration<UserImage>
{
    public void Configure(EntityTypeBuilder<UserImage> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.UserId).IsRequired();

        builder.Property(x => x.ImageUrl)
            .IsRequired(false);

        builder.Property(x => x.ImageData)
            .HasColumnType("varbinary(max)") // For SQL Server
            .IsRequired(false);              // Optional if not every record has it

    }
}
