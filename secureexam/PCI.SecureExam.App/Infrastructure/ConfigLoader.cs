using System.IO;
using Microsoft.Extensions.Configuration;
using PCI.SecureExam.Core;

namespace PCI.SecureExam.App.Infrastructure;

public static class ConfigLoader
{
    public static ClientConfig Load()
    {
        var cfg = new ClientConfig();
        try
        {
            var baseDir = AppContext.BaseDirectory;
            var builder = new ConfigurationBuilder()
                .SetBasePath(baseDir)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
                .AddJsonFile("appsettings.Local.json", optional: true, reloadOnChange: false)
                .AddEnvironmentVariables(prefix: "PCI_");
            builder.Build().Bind(cfg);
        }
        catch { /* fall back to defaults */ }
        return cfg;
    }
}
