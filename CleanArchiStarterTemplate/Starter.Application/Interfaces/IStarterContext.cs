using Microsoft.EntityFrameworkCore;
using Starter.Domain;

namespace Starter.Application.Interfaces;
public interface IStarterContext
{
    public DbSet<User> Users { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
