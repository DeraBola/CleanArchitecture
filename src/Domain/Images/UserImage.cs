using Domain.Images;
using Domain.Users;
using SharedKernel;

namespace Domain.Images;

public sealed class UserImage : Entity
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Uri? ImageUrl { get; set; } = null!;
    public byte[]? ImageData { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
   // public User? User { get; set; } // Navigation property
}
