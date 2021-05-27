using Microsoft.Extensions.DependencyInjection;
using RepChecker.Core;
using RepChecker.Helpers;
using RepChecker.MVVM.View;
using RepChecker.MVVM.ViewModel;
using System;
using System.Windows;

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
                mainWindow.DataContext = serviceProvider.GetRequiredService<MainViewModel>();
                mainWindow.Show();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
            }
        }

        private IServiceProvider CreateServiceProvider()
        {
            var services = new ServiceCollection();
            
            services.AddSingleton(this);
            services.AddTransient<MainWindow>();
            services.AddTransient<TestView>();
            services.AddTransient<TestViewModel>();
            services.AddTransient<IWindowFactory, WindowFactory>();
            services.AddTransient<MainViewModel>();
            services.AddTransient<ReputationViewModel>();


            return services.BuildServiceProvider();
        }
    }
}
