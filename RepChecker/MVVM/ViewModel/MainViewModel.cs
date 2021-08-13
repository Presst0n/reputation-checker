using Nito.AsyncEx;
using RepChecker.Core;
using RepChecker.Interfaces;
using RepChecker.MVVM.Model;
using RepChecker.Services;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RepChecker.MVVM.ViewModel
{
    public class MainViewModel : ViewModelBase, IWindowBehaviour, IMainViewModel
    {
        private readonly IWindowFactory _windowFactory;
        private readonly IApiService _apiService;
        private ViewModelBase _currentView = null;
        private LoggedInUserModel _loggedInUserModel;

        public event EventHandler<string> OnReputationFilter;
        public event EventHandler<bool> OnUserLogIn;
        public event EventHandler OnLogOut;

        public MainViewModel(IWindowFactory windowFactory, LoggedInUserModel loggedInUser, IApiService apiService)
        {
            LoggedInUserModel = loggedInUser;
            _apiService = apiService;
            _windowFactory = windowFactory;
        }

        public Action Close { get; set; }
        public Action Minimize { get; set; }

        private bool _isReputationsDropDownVisible;

        public bool IsReputationsDropDownVisible
        {
            get 
            { 
                return _isReputationsDropDownVisible; 
            }
            set 
            { 
                _isReputationsDropDownVisible = value;
                OnPropertyChanged();
            }
        }

        public LoggedInUserModel LoggedInUserModel
        {
            get => _loggedInUserModel;
            set
            {
                _loggedInUserModel = value;
                OnPropertyChanged();
            }
        }

        public ReputationViewModel ReputationVM { get; set; }

        public SettingsViewModel SettingsVM { get; set; }

        public ViewModelBase CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }

        public bool IsUserLoggedIn
        {
            get
            {
                return LoggedInUserModel.IsLoggedIn;
            }
            set
            {
                LoggedInUserModel.IsLoggedIn = value;
                OnPropertyChanged();
            }
        }

        private bool _repButtonVisible;

        public bool RepButtonVisible
        {
            get => _repButtonVisible;
            set
            {
                _repButtonVisible = value;
                OnPropertyChanged();
            }
        }

        private string _loggedInUserName;

        public string LoggedInUserName
        {
            get
            {
                return _loggedInUserName;
            }
            set
            {
                _loggedInUserName = value != null ? $"Hello {value}" : null;
                OnPropertyChanged();
            }
        }

        public ICommand MinimizeApp => new RelayCommand<string>(mode =>
        {
            MinimizeApplication();
        });

        public ICommand CloseApp => new RelayCommand<string>(mode =>
        {
            CloseApplication();
        });

        public ICommand LogIn => new RelayCommand<string>(mode =>
        {
            var result = AsyncContext.Run(_apiService.AuthorizeAsync);

            if (!result)
                return;

            var userResponse = AsyncContext.Run(() => _apiService.GetUserInfoAsync());

            if (string.IsNullOrEmpty(userResponse.BattleTag))
                return;

            _loggedInUserModel.BattleTag = userResponse.BattleTag;
            LoggedInUserName = userResponse.BattleTag;
            IsUserLoggedIn = true;

            OnUserLogIn?.Invoke(this, true);
            OnPropertyChanged();
        });

        public ICommand DisplayReputationPage => new RelayCommand<string>(mode =>
        {
            if (ReputationVM is null)
            {
                ReputationVM = _windowFactory.GetViewModel<ReputationViewModel>();
                ReputationVM.OnLoadingReputationsCompleted += ReputationVM_OnLoadingReputationsCompleted;
            }

            if (CurrentView == ReputationVM)
                return;

            if (_loggedInUserModel.IsLoggedIn)
            {
                Task.Run(async () => await ReputationVM.LoadReputations());
            }

            CurrentView = ReputationVM;

            OnPropertyChanged();
        });

        private void ReputationVM_OnLoadingReputationsCompleted(object sender, bool e)
        {
            RepButtonVisible = e;
        }

        public ICommand DisplaySettingsPage => new RelayCommand<string>(mode =>
        {
            if (SettingsVM is null)
                SettingsVM = _windowFactory.GetViewModel<SettingsViewModel>();

            if (CurrentView == SettingsVM)
                return;

            CurrentView = SettingsVM;

            OnPropertyChanged();
        });

        public ICommand ShowExaltedReputations => new RelayCommand<string>(mode =>
        {
            OnReputationFilter?.Invoke(this, "Exalted");

            OnPropertyChanged();
        });

        public ICommand ShowReveredReputations => new RelayCommand<string>(mode =>
        {
            OnReputationFilter?.Invoke(this, "Revered");

            OnPropertyChanged();
        });

        public ICommand ShowHonoredReputations => new RelayCommand<string>(mode =>
        {
            OnReputationFilter?.Invoke(this, "Honored");

            OnPropertyChanged();
        });

        public ICommand ShowFriendlyReputations => new RelayCommand<string>(mode =>
        {
            OnReputationFilter?.Invoke(this, "Friendly");

            OnPropertyChanged();
        });

        public ICommand ShowNeutralReputations => new RelayCommand<string>(mode =>
        {
            OnReputationFilter?.Invoke(this, "Neutral");

            OnPropertyChanged();
        });

        public ICommand ShowUnfriendlyReputations => new RelayCommand<string>(mode =>
        {
            OnReputationFilter?.Invoke(this, "Unfriendly");

            OnPropertyChanged();
        });

        public ICommand ShowHostileReputations => new RelayCommand<string>(mode =>
        {
            OnReputationFilter?.Invoke(this, "Hostile");

            OnPropertyChanged();
        });

        public ICommand ShowHatedReputations => new RelayCommand<string>(mode =>
        {
            OnReputationFilter?.Invoke(this, "Hated");

            OnPropertyChanged();
        });

        public ICommand LogOut => new RelayCommand<string>(mode =>
        {
            // TODO: Clean up this later.

            RepButtonVisible = false;
            IsReputationsDropDownVisible = false;
            OnLogOut?.Invoke(this, EventArgs.Empty);
            ReputationVM.OnLoadingReputationsCompleted -= ReputationVM_OnLoadingReputationsCompleted;
            CurrentView = null;
            ReputationVM = null;
            SettingsVM = null;
            _loggedInUserModel.BattleTag = null;
            _loggedInUserModel.IsLoggedIn = false;
            IsUserLoggedIn = false;
            LoggedInUserName = null;

            OnPropertyChanged();
        });

        public void CloseApplication()
        {
            Close?.Invoke();
        }

        public void MinimizeApplication()
        {
            Minimize?.Invoke();
        }
    }
}
