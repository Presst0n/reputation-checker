using System;

namespace RepChecker.MVVM.ViewModel
{
    public interface IMainViewModel
    {
        event EventHandler<string> OnReputationFilter;
    }
}