using Microsoft.Extensions.DependencyInjection;
using RepChecker.Core;
using RepChecker.MVVM.Model;
using RepChecker.MVVM.View;
using RepChecker.MVVM.ViewModel;
using System;
using System.Windows;
using RepChecker.Data;
using RepChecker.Repository;
using RepChecker.Services;
using RepChecker.Settings;
using Microsoft.Extensions.Hosting;

namespace RepChecker
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly IHost _host;

        public App()
        {
            _host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    ConfigureServices(services);
                }).Build();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<LoggedInUserModel>();
            services.AddScoped<IStandingsRepository, StandingsRepository>();
            services.AddScoped<IApplicationSettings, ApplicationSettings>();
            services.AddScoped<IApiService, ApiService>();
            services.AddScoped<MainWindow>();
            services.AddScoped<IMainViewModel, MainViewModel>();

            services.AddTransient<SettingsView>();
            services.AddTransient<SettingsViewModel>();
            services.AddTransient<IWindowFactory, WindowFactory>();
            services.AddTransient<ReputationViewModel>();

            services.AddDbContext<ReputationDbContext>(ServiceLifetime.Scoped);

            services.AddAutoMapper(typeof(App));
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            MainWindow mainWindow = _host.Services.GetRequiredService<MainWindow>();

            try
            {
                mainWindow.DataContext = _host.Services.GetRequiredService<IMainViewModel>();
                mainWindow.Show();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
            }

            await _host.StartAsync();

            base.OnStartup(e);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            using (_host)
            {
                await _host.StopAsync();
            }

            base.OnExit(e);
        }

    }
}
