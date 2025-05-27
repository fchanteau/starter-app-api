using Microsoft.EntityFrameworkCore;
using Starter.Application.Interfaces;
using Starter.Domain;

namespace Starter.Infrastructure.Database;
public class StarterContext : DbContext, IStarterContext
{
    public StarterContext()
    {

    }

    public StarterContext(DbContextOptions<StarterContext> options) : base(options)
    {
    }

    public virtual DbSet<User> Users { get; set; }
}
