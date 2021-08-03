using Nito.AsyncEx;
using RepChecker.Core;
using RepChecker.Extensions;
using RepChecker.Interfaces;
using RepChecker.MVVM.Model;
using RepChecker.Services;
using System;
using System.Linq;
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

        public MainViewModel(IWindowFactory windowFactory, LoggedInUserModel loggedInUser, IApiService apiService)
        {
            LoggedInUserModel = loggedInUser;
            _apiService = apiService;
            _windowFactory = windowFactory;
        }

        public Action Close { get; set; }
        public Action Minimize { get; set; }

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

        public string LoggedInUserName
        {
            get
            {
                return $"Hello {LoggedInUserModel.BattleTag}!";
            }
            set
            {
                LoggedInUserModel.BattleTag = value;
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

            LoggedInUserName = userResponse.BattleTag;
            IsUserLoggedIn = true;

            OnPropertyChanged();
        });

        public ICommand DisplayReputationPage => new RelayCommand<string>(mode =>
        {
            if (ReputationVM is null)
            {
                ReputationVM = _windowFactory.GetViewModel<ReputationViewModel>();
                ReputationVM.OnLoadingReputationsCompleted += (obj, e) => { RepButtonVisible = e; };
            }

            if (CurrentView == ReputationVM)
                return;

            Task.Run(async () => await ReputationVM.LoadReputations());

            CurrentView = ReputationVM;

            OnPropertyChanged();
        });

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
            //if (ReputationVM?.ReputationsCollection is null)
            //    return;
            //var filteredCollection = ReputationVM?.ReputationsCollection.Where(x => x.Standing.Level == "Exalted").ToObservableCollection();
            //ReputationVM.TestModels = filteredCollection;

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
