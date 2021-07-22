using RepChecker.Core;
using RepChecker.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepChecker.MVVM.ViewModel
{
    public class SettingsViewModel : ViewModelBase
    {

        public List<object> RefreshTimeSpan { get; set; } = new List<object>
        {
            new { Name = "30 min", Id = 1, RefreshmentTime = TimeSpan.FromMinutes(30)},
            new { Name = "1h", Id = 2, RefreshmentTime = TimeSpan.FromMinutes(60)},
            new { Name = "2hrs", Id = 3, RefreshmentTime = TimeSpan.FromMinutes(120)},
            new { Name = "3hrs", Id = 4, RefreshmentTime = TimeSpan.FromMinutes(180)}
        };

        public object SelectedRefreshTimeSpan { get; set; }

        public SettingsViewModel()
        {

        }
    }
}
