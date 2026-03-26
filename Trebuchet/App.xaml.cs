using Microsoft.Extensions.Configuration;
using System.Windows;

namespace Trebuchet
{
    public partial class App : Application
    {
        public static IConfiguration Configuration { get; private set; } = null!;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

#if DEBUG
            const string configFile = "LauncherConfig.debug.json";
#else
            const string configFile = "LauncherConfig.json";
#endif

            Configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile(configFile, optional: false, reloadOnChange: true)
                .Build();

            new MainWindow().Show();
        }
    }
}