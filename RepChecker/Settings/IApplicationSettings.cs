using System;

namespace RepChecker.Settings
{
    public interface IApplicationSettings
    {
        TimeSpan GetDataRefreshTimeValue();
        void RestoreDefaultSettings();
        void SetDataRefreshTimeValue(TimeSpan timeSpan);
    }
}