using Domain.Images;
using Domain.Todos;
using Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Application.Abstractions.Data;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; }
    DbSet<TodoItem> TodoItems { get; }
    DbSet<UserImages> UserImages{ get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
