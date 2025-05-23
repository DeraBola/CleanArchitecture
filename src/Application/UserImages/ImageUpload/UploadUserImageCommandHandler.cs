using System.Net;
using System.Runtime.Intrinsics.X86;
using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Images;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using SharedKernel;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Application.Images.ImageUpload;

internal sealed class UploadUserImageCommandHandler(IApplicationDbContext context, 
    IDateTimeProvider dateTimeProvider,
    IUserContext userContext)
    : ICommandHandler<UploadUserImageCommand, Guid>
{
    public async Task<Result<Guid>> Handle(UploadUserImageCommand command, CancellationToken cancellationToken)
    {
        if (userContext.UserId != command.UserId)
        {
            return Result.Failure<Guid>(UserErrors.Unauthorized());
        }

        User? user = await context.Users.AsNoTracking()
          .SingleOrDefaultAsync(u => u.Id == command.UserId, cancellationToken);

        if (user is null)
        {
            return Result.Failure<Guid>(UserErrors.NotFound(command.UserId));
        }

        var image = new UserImages
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            ImageData = command.ImageData,
            CreatedAt = dateTimeProvider.UtcNow
        };

        // Step 3: Add to DB and save
        context.UserImages.Add(image);
        await context.SaveChangesAsync(cancellationToken);

        // Step 4: Return result
        return Result.Success(image.Id);
    }
}
