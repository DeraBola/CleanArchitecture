using Application.Abstractions.Messaging;
using Domain.Todos;

namespace Application.Images.ImageUpload;

public sealed record UploadUserImageCommand : ICommand<Guid>
{
    public Guid UserId { get; set; }
    public byte[]? ImageData { get; set; }
    public Uri ImageUrl { get; set; } = null!;
}
