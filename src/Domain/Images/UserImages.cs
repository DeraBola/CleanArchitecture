using Domain.Images;
using Domain.Users;
using SharedKernel;

namespace Domain.Images;

public sealed class UserImages : Entity
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Uri ImageUrl { get; set; } = null!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
   // public User? User { get; set; } // Navigation property
}
