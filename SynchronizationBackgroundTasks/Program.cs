using FolderSynchronization.Services;
using SynchronizationBackgroundTask;

class Program {

    static void Main(string[] args) {

        if (args.Length == 0)
        {
            SynchronizationService.ConsoleWriteInColor(SynchronizationService.GetMessageNoArgumentsProvided(), ConsoleColor.Red);
    
        } else if (args.Length < 4) {

            SynchronizationService.ConsoleWriteInColor(SynchronizationService.GetMessageProvideArguments(), ConsoleColor.Red);
        }
        else
        {
            var builder = Host.CreateApplicationBuilder(args);

            try 
            {
                builder.Services.AddSingleton<IHostedService>(provider => new TimedHostedService(args
                , provider.GetService<ILogger<TimedHostedService>>()));

                var host = builder.Build();
                host.Run();
            }
            catch (Exception ex) {
                SynchronizationService.ConsoleWriteInColor(ex.Message, ConsoleColor.Red);
            }
            
        }
    }
}

