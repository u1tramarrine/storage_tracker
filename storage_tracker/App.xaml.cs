using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using storage_tracker.Data;
using storage_tracker.Services;
using storage_tracker.ViewModels;
using storage_tracker.Views;
using System.Configuration;
using System.Data;
using System.IO;
using System.Windows;

namespace storage_tracker
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IServiceProvider _serviceProvider;
        public IServiceProvider? ServiceProvider => _serviceProvider;
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var services = new ServiceCollection();
            ConfigureServices(services);
            _serviceProvider = services.BuildServiceProvider();

            var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            // Configuration
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            // Database
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("PostgresConnection"),
                    npgsqlOptions => npgsqlOptions.MigrationsHistoryTable("__EFMigrationsHistory", "public")));

            // Repositories
            services.AddScoped<IRepository<Models.Category>, CategoryRepository>();
            services.AddScoped<IRepository<Models.Box>, BoxRepository>();
            services.AddScoped<IRepository<Models.Item>, ItemRepository>();

            // Services
            services.AddSingleton<IDialogService, DialogService>();

            // ViewModels
            services.AddTransient<MainViewModel>();

            // Views
            services.AddTransient<MainWindow>();
        }
    }

}
