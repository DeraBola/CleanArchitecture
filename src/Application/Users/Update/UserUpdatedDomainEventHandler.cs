using Domain.Users;
using MediatR;

namespace Application.Users.Update;

internal sealed class UserUpdatedDomainEventHandler : INotificationHandler<UserUpdatedDomainEvent>
{
    public Task Handle(UserUpdatedDomainEvent notification, CancellationToken cancellationToken)
    {
        // TODO: Send an email verification link, etc.
        return Task.CompletedTask;
    }
}
