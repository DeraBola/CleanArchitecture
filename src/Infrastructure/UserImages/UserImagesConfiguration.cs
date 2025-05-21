using Domain.Images;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Images;

internal sealed class UserImageConfiguration : IEntityTypeConfiguration<UserImages>
{

    public void Configure(EntityTypeBuilder<UserImages> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.UserId).IsRequired();

        builder.Property(x => x.ImageUrl)
            .IsRequired();
    }
}
