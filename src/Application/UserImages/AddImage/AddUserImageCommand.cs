using Application.Abstractions.Messaging;
using Domain.Todos;

namespace Application.Images.AddImage;

public sealed record AddUserImageCommand : ICommand<Guid>
{
    public Guid UserId { get; set; }
    public Uri ImageUrl { get; set; }
}
