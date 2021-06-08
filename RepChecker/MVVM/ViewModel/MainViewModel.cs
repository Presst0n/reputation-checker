using RepChecker.Core;
using RepChecker.MVVM.Model;
using RepChecker.MVVM.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using RepChecker.Extensions;
using RepDataCollector.Core;
using Nito.AsyncEx;
using System.Threading.Tasks;
using RepChecker.Enums;

namespace RepChecker.MVVM.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IWindowFactory _windowFactory;
        private readonly BattleNetApiRequestsHandler _apiClient;
        private ViewModelBase _currentView = null;
        private LoggedInUserModel _loggedInUserModel;

        public MainViewModel(IWindowFactory windowFactory, LoggedInUserModel loggedInUser, BattleNetApiRequestsHandler apiClient)
        {
            LoggedInUserModel = loggedInUser;
            _apiClient = apiClient;
            _windowFactory = windowFactory;
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
        public TestViewModel TestVM { get; set; }

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

        public ICommand LogIn => new RelayCommand<string>(mode =>
        {
            // TODO - Handle logging via battle.net
            var result = AsyncContext.Run(_apiClient.AuthorizeAsync);

            if (!result)
                return;

            var userResponse = AsyncContext.Run(() => _apiClient.GetUserInfoAsync());

            LoggedInUserName = userResponse.BattleTag;
            IsUserLoggedIn = true;

            OnPropertyChanged();
        });

        public ICommand DisplayReputationPage => new RelayCommand<string>(mode =>
        {
            if (ReputationVM is null)
            {
                ReputationVM = _windowFactory.GetViewModel<ReputationViewModel>();
                ReputationVM.LoadingReputationsCompleted += (obj, e) => { RepButtonVisible = e; };
            }

            if (CurrentView == ReputationVM)
                return;
            // Check if that prevents freezing UI Thread.
            Task.Run(async () => await ReputationVM.LoadReputations());
            //Task.Run(() => ReputationVM.LongRunningTestTask());
            //ReputationVM.LongRunningTestTask();
            CurrentView = ReputationVM;
            
            OnPropertyChanged();
        });

        public void OnLoadCompleted()
        {
            RepButtonVisible = true;
        }

        public ICommand DisplayTestPage => new RelayCommand<string>(mode =>
        {
            //if (TestVM is null)
            //    TestVM = _windowFactory.GetViewModel<TestViewModel>();

            //if (CurrentView == TestVM)
            //    return;

            //CurrentView = TestVM;

            //OnPropertyChanged();
        });

        public ICommand ShowExaltedReputations => new RelayCommand<string>(mode =>
        {
            //ReputationVM.TestModels = ReputationVM?.AllReputations.Where(x => x.Standing.Level == "Exalted").ToObservableCollection();

            if (ReputationVM?.ReputationsCollection is null)
                return;

            ReputationVM.TestModels = ReputationVM.ReputationsCollection.Exalted.ToObservableCollection();

            OnPropertyChanged();
        });

        public ICommand ShowReveredReputations => new RelayCommand<string>(mode =>
        {
            if (ReputationVM?.TestModels is null)
                return;
            //ReputationVM.TestModels = ReputationVM.AllReputations.Where(x => x.Standing.Level == "Revered").ToObservableCollection();
            ReputationVM.TestModels = ReputationVM.ReputationsCollection.Revered.ToObservableCollection();
            //ReputationVM.ShowReputations(ReputationTiers.Revered);
            OnPropertyChanged();
        });

        public ICommand ShowHonoredReputations => new RelayCommand<string>(mode =>
        {
            if (ReputationVM?.TestModels is null)
                return;
            //ReputationVM.TestModels = ReputationVM.AllReputations.Where(x => x.Standing.Level == "Honored").ToObservableCollection();
            ReputationVM.TestModels = ReputationVM.ReputationsCollection.Honored.ToObservableCollection();
            //ReputationVM.ShowReputations(ReputationTiers.Honored);
            OnPropertyChanged();
        });

        public ICommand ShowFriendlyReputations => new RelayCommand<string>(mode =>
        {
            if (ReputationVM?.TestModels is null)
                return;
            //ReputationVM.TestModels = ReputationVM.AllReputations.Where(x => x.Standing.Level == "Friendly").ToObservableCollection();
            ReputationVM.TestModels = ReputationVM.ReputationsCollection.Friendly.ToObservableCollection();
            //ReputationVM.ShowReputations(ReputationTiers.Friendly);
            OnPropertyChanged();
        });

        public ICommand ShowNeutralReputations => new RelayCommand<string>(mode =>
        {
            if (ReputationVM?.TestModels is null)
                return;
            //ReputationVM.TestModels = ReputationVM.AllReputations.Where(x => x.Standing.Level == "Neutral").ToObservableCollection();
            ReputationVM.TestModels = ReputationVM.ReputationsCollection.Neutral.ToObservableCollection();
            //ReputationVM.ShowReputations(ReputationTiers.Neutral);
            OnPropertyChanged();
        });

        public ICommand ShowUnfriendlyReputations => new RelayCommand<string>(mode =>
        {
            if (ReputationVM?.TestModels is null)
                return;
            //var test = _windowFactory.GetUserControl<TestView>();
            //test.DataContext = _testVM;
            //CurrentView = _testVM;

            //ReputationVM.TestModels = ReputationVM.AllReputations.Where(x => x.Standing.Level == "Unfriendly").ToObservableCollection();
            ReputationVM.TestModels = ReputationVM.ReputationsCollection.Unfriendly.ToObservableCollection();
            //ReputationVM.ShowReputations(ReputationTiers.Unfriendly);
            OnPropertyChanged();
        });

        public ICommand ShowHostileReputations => new RelayCommand<string>(mode =>
        {
            if (ReputationVM?.TestModels is null)
                return;
            //ReputationVM.TestModels = ReputationVM.AllReputations.Where(x => x.Standing.Level == "Hostile").ToObservableCollection();
            ReputationVM.TestModels = ReputationVM.ReputationsCollection.Hostile.ToObservableCollection();
            //ReputationVM.ShowReputations(ReputationTiers.Hostile);
            OnPropertyChanged();
        });

        public ICommand ShowHatedReputations => new RelayCommand<string>(mode =>
        {
            if (ReputationVM?.TestModels is null)
                return;
            //ReputationVM.TestModels = ReputationVM.AllReputations.Where(x => x.Standing.Level == "Hated").ToObservableCollection();
            ReputationVM.TestModels = ReputationVM.ReputationsCollection.Hated.ToObservableCollection();
            //ReputationVM.ShowReputations(ReputationTiers.Hated);
            OnPropertyChanged();
        });
    }
}
