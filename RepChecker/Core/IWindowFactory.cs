using System.Windows;
using System.Windows.Controls;

namespace RepChecker.Core
{
    public interface IWindowFactory
    {
        T GetUserControl<T>() where T : UserControl;
        T GetWindow<T>() where T : Window;
    }
}