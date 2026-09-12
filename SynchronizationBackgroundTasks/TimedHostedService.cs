
using FolderSynchronization.Services;
using FolderSynchronization.Models;

namespace SynchronizationBackgroundTask
{
    internal class TimedHostedService : BackgroundService
    {
        private readonly ILogger<TimedHostedService> _logger;
        private int _executionCount;
        private ApplicationParameters _applicationParameters;

        public TimedHostedService(string[] args, ILogger<TimedHostedService> logger)
        {
            _applicationParameters = SynchronizationService.PopulateParameters(args);
            _logger = logger;
        }

        public TimedHostedService(ApplicationParameters param, ILogger<TimedHostedService> logger)
        {
            _applicationParameters = param;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Service is running.");

            try
            {
                await DoWork();

                using PeriodicTimer timer = new(TimeSpan.FromSeconds(_applicationParameters.Interval));

                while (await timer.WaitForNextTickAsync(stoppingToken))
                {
                    await DoWork();
                }
            }
            catch (Exception ex) {
                SynchronizationService.ConsoleWriteInColor(ex.Message, ConsoleColor.Red);
                _logger.LogInformation("Service is stopping.");
                Environment.Exit(1);
            }
        }

        private async Task DoWork()
        {
            new SynchronizationService(_applicationParameters).Sync();
        }
    }
}
