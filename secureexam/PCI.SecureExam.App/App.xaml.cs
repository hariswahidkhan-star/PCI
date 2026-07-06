using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PCI.SecureExam.App.Infrastructure;
using PCI.SecureExam.Core;

namespace PCI.SecureExam.App;

public partial class App : Application
{
    public static IServiceProvider Services { get; private set; } = default!;
    public static ClientConfig Config { get; private set; } = new();

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        Config = ConfigLoader.Load();

        var sc = new ServiceCollection();
        sc.AddLogging(b => b.AddSimpleConsole(o => o.SingleLine = true).SetMinimumLevel(LogLevel.Information));
        sc.AddSingleton(Config);
        Services = sc.BuildServiceProvider();

        if (Config.Features.RegisterUriScheme)
            UriSchemeRegistrar.EnsureRegistered();

        // Headless self-test: `PCISecureExam.exe --selftest` verifies the machine and exits (0 = ready).
        if (e.Args.Any(a => a.Equals("--selftest", StringComparison.OrdinalIgnoreCase)))
        {
            var code = SelfTest.Run();
            Shutdown(code);
            return;
        }
    }
}
