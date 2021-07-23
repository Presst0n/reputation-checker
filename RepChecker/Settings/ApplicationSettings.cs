using System;
using System.Collections.Generic;
using System.Text;

namespace RepChecker.Settings
{
    public class ApplicationSettings : IApplicationSettings
    {

        public TimeSpan GetDataRefreshValue()
        {
            var value = Properties.Settings.Default.DataRefresh;

            return value;
        }

        public void SetDataRefreshValue(TimeSpan timeSpan)
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
