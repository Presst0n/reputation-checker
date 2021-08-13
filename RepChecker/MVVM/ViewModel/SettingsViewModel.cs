using RepChecker.Core;
using RepChecker.Enums;
using RepChecker.MVVM.Model;
using RepChecker.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RepChecker.MVVM.ViewModel
{
    public class SettingsViewModel : ViewModelBase
    {
        private readonly IApplicationSettings _applicationSettings;

        //private const string Exalted = "Exalted";

        public List<RefreshTimeModel> RefreshTimeSpan { get; set; } = new List<RefreshTimeModel>()
        {
            new RefreshTimeModel() { Name = "30 min", Id = 1, RefreshmentTime = TimeSpan.FromMinutes(30)},
            new RefreshTimeModel() { Name = "1h", Id = 2, RefreshmentTime = TimeSpan.FromMinutes(60)},
            new RefreshTimeModel() { Name = "2hrs", Id = 3, RefreshmentTime = TimeSpan.FromMinutes(120)},
            new RefreshTimeModel() { Name = "3hrs", Id = 4, RefreshmentTime = TimeSpan.FromMinutes(180)}
        };

        ~SettingsViewModel()
        {

        }

        private RefreshTimeModel _selectedRefreshTimeSpan;

        public RefreshTimeModel SelectedRefreshTimeSpan
        {
            get
            {
                if (_selectedRefreshTimeSpan is null)
                {
                    return RefreshTimeSpan.First(x => x.RefreshmentTime == _applicationSettings.GetDataRefreshTimeValue());
                }

                return _selectedRefreshTimeSpan;
            }
            set 
            {
                _selectedRefreshTimeSpan = value;
                OnPropertyChanged();
                //var testttorro = Exalted;
                _applicationSettings.SetDataRefreshTimeValue(_selectedRefreshTimeSpan.RefreshmentTime);
            }
        }

        public SettingsViewModel(IApplicationSettings applicationSettings)
        {
            _applicationSettings = applicationSettings;
        }
    }
}
