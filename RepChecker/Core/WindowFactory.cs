using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace RepChecker.Core
{
    public class WindowFactory : IWindowFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public WindowFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public T GetWindow<T>() where T : Window
        {
            return (T)_serviceProvider.GetRequiredService(typeof(T));
        }

        public T GetUserControl<T>() where T : UserControl
        {
            return (T)_serviceProvider.GetRequiredService(typeof(T));
        }
    }
}
