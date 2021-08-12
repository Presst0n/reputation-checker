using System;
using System.Collections.Generic;
using System.Text;

namespace RepChecker.Settings
{
    public class ApplicationSettings : IApplicationSettings
    {

        public TimeSpan GetDataRefreshTimeValue()
        {
            return Properties.Settings.Default.DataRefresh;
        }

        public void SetDataRefreshTimeValue(TimeSpan timeSpan)
        {
            Properties.Settings.Default.DataRefresh = timeSpan;
            Properties.Settings.Default.Save();
        }

        public void RestoreDefaultSettings()
        {
            Properties.Settings.Default.DataRefresh = TimeSpan.FromMinutes(30);
            Properties.Settings.Default.Save();
        }
    }
}
