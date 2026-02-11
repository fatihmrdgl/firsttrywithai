using Microsoft.EntityFrameworkCore;
using MimCrm.Api.Data;

namespace MimCrm.Api.Services;

public class SubscriptionBackgroundJob(AppDbContext dbContext, ILogger<SubscriptionBackgroundJob> logger)
{
    public async Task ValidateAndFlagExpiredSubscriptions()
    {
        var now = DateTime.UtcNow;
        var expiredPlans = await dbContext.SubscriptionPlans
            .Where(x => x.IsActive && x.EndsAtUtc < now)
            .ToListAsync();

        if (expiredPlans.Count == 0)
        {
            logger.LogInformation("No expired subscriptions found at {TimestampUtc}", now);
            return;
        }

        foreach (var plan in expiredPlans)
        {
            plan.IsActive = false;
        }

        await dbContext.SaveChangesAsync();
        logger.LogInformation("Flagged {Count} subscription(s) as expired.", expiredPlans.Count);
    }
}
