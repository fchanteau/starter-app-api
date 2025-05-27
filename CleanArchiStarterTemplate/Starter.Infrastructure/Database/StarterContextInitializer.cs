using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Starter.Infrastructure.Database;
public class StarterContextInitializer(StarterContext dbContext, ILogger<StarterContextInitializer> logger)
{
    public async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await dbContext.Database.MigrateAsync(cancellationToken);
        }
        catch(Exception ex)
        {
            logger.LogError(ex, "An error occurred while initialising the database.");
            throw;
        }
    }
}
