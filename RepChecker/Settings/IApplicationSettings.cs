using System;

namespace RepChecker.Settings
{
    public interface IApplicationSettings
    {
        TimeSpan GetDataRefreshValue();
        void RestoreDefaultSettings();
        void SetDataRefreshValue(TimeSpan timeSpan);
    }
}