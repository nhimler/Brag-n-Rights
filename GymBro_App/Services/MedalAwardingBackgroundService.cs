using GymBro_App.DAL.Abstract;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace GymBro_App.Services
{
    public class MedalAwardingBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<MedalAwardingBackgroundService> _logger;
        private static readonly TimeSpan Interval = TimeSpan.FromMinutes(30);

        public MedalAwardingBackgroundService(IServiceProvider serviceProvider, ILogger<MedalAwardingBackgroundService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    // Use a scope only for fetching users and checking tokens
                    using var scope = _serviceProvider.CreateScope();
                    var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
                    var oAuthService = scope.ServiceProvider.GetRequiredService<IOAuthService>();

                    var userIds = await userRepository.GetAllUserIdentityIDAsync();

                    var validUserIds = new List<string>();
                    foreach (var userId in userIds)
                    {
                        if (await oAuthService.UserHasFitbitToken(userId))
                        {
                            validUserIds.Add(userId);
                        }
                    }

                    // Process medal awarding in parallel
                    var tasks = validUserIds.Select(async userId =>
                    {
                        try
                        {
                            // No scope here; AwardUserdMedalsAsync should handle scoped dependencies internally
                            _logger.LogInformation($"Awarding medals for user ID: {userId}");
                            using var taskScope = _serviceProvider.CreateScope();
                            var awardMedalService = taskScope.ServiceProvider.GetRequiredService<IAwardMedalService>();

                            await awardMedalService.AwardUserdMedalsAsync(userId);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, $"Error awarding medal to user {userId}");
                        }
                    });

                    await Task.WhenAll(tasks);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error in ExecuteAsync method of MedalAwardingBackgroundService");
                }

                await Task.Delay(Interval, stoppingToken);
            }
        }
    }
}
