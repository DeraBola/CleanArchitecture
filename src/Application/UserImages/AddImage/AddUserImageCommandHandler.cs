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

namespace Application.Images.AddImage;

internal sealed class AddUserImageCommandHandler(IApplicationDbContext context, 
    IDateTimeProvider dateTimeProvider,
    IUserContext userContext)
    : ICommandHandler<AddUserImageCommand, Guid>
{
    public async Task<Result<Guid>> Handle(AddUserImageCommand command, CancellationToken cancellationToken)
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

        var image = new UserImage
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            ImageUrl = command.ImageUrl.ToString(),
            CreatedAt = dateTimeProvider.UtcNow
        };

        // Step 3: Add to DB and save
        context.UserImages.Add(image);
        await context.SaveChangesAsync(cancellationToken);

        // Step 4: Return result
        return Result.Success(image.Id);
    }
}
