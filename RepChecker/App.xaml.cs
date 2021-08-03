using RepDataCollector.Core;
using Microsoft.Extensions.DependencyInjection;
using RepChecker.Core;
using RepChecker.Helpers;
using RepChecker.MVVM.Model;
using RepChecker.MVVM.View;
using RepChecker.MVVM.ViewModel;
using System;
using System.Configuration;
using System.Windows;
using RepChecker.Data;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using System.Reflection;
using RepChecker.Repository;
using RepChecker.Services;
using RepChecker.Settings;

namespace RepChecker
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            // Find out if I should use IHost Builder https://www.youtube.com/watch?v=XW_qgbUg1ZI (Video title: Adding Dependency Injection to WPF applications)

            IServiceProvider serviceProvider = CreateServiceProvider();
            var mainWindow = serviceProvider.GetRequiredService<MainWindow>();

            try
            {
                mainWindow.DataContext = serviceProvider.GetRequiredService<IMainViewModel>();
                mainWindow.Show();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
            }
        }

        private IServiceProvider CreateServiceProvider()
        {
            var services = AddDependencies(new ServiceCollection());
            return services.BuildServiceProvider();
        }


        private ServiceCollection AddDependencies(ServiceCollection services)
        {
            //var apiClient = ConfigureApiClient();

            services.AddSingleton(this);
            services.AddScoped<LoggedInUserModel>();
            services.AddScoped<IApplicationSettings, ApplicationSettings>();
            services.AddScoped<IApiService, ApiService>();
            services.AddScoped<MainWindow>();
            services.AddScoped<TestView>();
            services.AddScoped<SettingsView>();
            services.AddScoped<SettingsViewModel>();
            services.AddScoped<IWindowFactory, WindowFactory>();
            services.AddScoped<IMainViewModel, MainViewModel>();
            services.AddScoped<ReputationViewModel>();
            services.AddScoped<IStandingsRepository, StandingsRepository>();

            //services.AddDbContext<ReputationDbContext>(options => 
            //{
            //    options.UseSqlite("Data Source = reputations.db");
            //});

            //var config = new MapperConfiguration(cfg =>
            //{
            //    cfg.AddMaps(Assembly.GetExecutingAssembly());
            //});
            //IMapper mapper = new Mapper(config);

            services.AddDbContext<ReputationDbContext>(ServiceLifetime.Scoped);
            //services.AddSingleton(mapper);
            services.AddAutoMapper(typeof(App));


            return services;
        }
    }
}
