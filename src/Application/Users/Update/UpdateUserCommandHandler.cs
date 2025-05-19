using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Users.Update;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Users.Update;

internal sealed class UpdateUserCommandHandler(IApplicationDbContext context)
    : ICommandHandler<UpdateUserCommand, Guid>
{

    public async Task<Result<Guid>> Handle(UpdateUserCommand command, CancellationToken cancellationToken)
    {
        User? user = await context.Users.FindAsync(new object[] { command.Id }, cancellationToken);

        if (user is null)
        {
            return Result.Failure<Guid>(UserErrors.NotFound(command.Id));
        }

        bool emailTaken = await context.Users
            .AnyAsync(u => u.Email == command.Email && u.Id != command.Id, cancellationToken);

        if (emailTaken)
        {
            return Result.Failure<Guid>(UserErrors.EmailNotUnique);
        }

        user.Email = command.Email;
        user.FirstName = command.FirstName;
        user.LastName = command.LastName;

        user.Raise(new UserUpdatedDomainEvent(user.Id));

        await context.SaveChangesAsync(cancellationToken);

        return user.Id;
    }
}
