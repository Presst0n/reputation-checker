using System;

namespace RepChecker.MVVM.ViewModel
{
    public interface IMainViewModel
    {
        event EventHandler<string> OnReputationFilter;
        event EventHandler<bool> OnUserLogIn;
        event EventHandler OnLogOut;
    }
}