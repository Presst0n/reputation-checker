using RepChecker.Core;
using RepChecker.MVVM.Model;
using RepChecker.MVVM.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using RepChecker.Extensions;

namespace RepChecker.MVVM.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IWindowFactory _windowFactory;
        private ViewModelBase _currentView = null;
        private bool _isUserLoggedIn = true;

        public MainViewModel(IWindowFactory windowFactory)
        {
            _windowFactory = windowFactory;
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
                return _isUserLoggedIn;
            }
            set
            {
                _isUserLoggedIn = value;
                OnPropertyChanged(nameof(IsUserLoggedIn));
            }
        }

        public ICommand DisplayReputationPage => new RelayCommand<string>(mode =>
        {
            if (ReputationVM is null)
                ReputationVM = _windowFactory.GetViewModel<ReputationViewModel>();

            if (CurrentView == ReputationVM)
                return;
            
            CurrentView = ReputationVM;

            OnPropertyChanged();
        });

        public ICommand DisplayTestPage => new RelayCommand<string>(mode =>
        {
            if (TestVM is null)
                TestVM = _windowFactory.GetViewModel<TestViewModel>();

            if (CurrentView == TestVM)
                return;

            CurrentView = TestVM;

            OnPropertyChanged();
        });

        public ICommand ShowExaltedReputations => new RelayCommand<string>(mode =>
        {
            ReputationVM.TestModels = ReputationVM?.AllReputations.Where(x => x.Standing.Level == "Exalted").ToObservableCollection();

            OnPropertyChanged();
        });

        public ICommand ShowReveredReputations => new RelayCommand<string>(mode =>
        {
            ReputationVM.TestModels = ReputationVM.AllReputations.Where(x => x.Standing.Level == "Revered").ToObservableCollection();

            OnPropertyChanged();
        });

        public ICommand ShowHonoredReputations => new RelayCommand<string>(mode =>
        {
            ReputationVM.TestModels = ReputationVM.AllReputations.Where(x => x.Standing.Level == "Honored").ToObservableCollection();

            OnPropertyChanged();
        });

        public ICommand ShowFriendlyReputations => new RelayCommand<string>(mode =>
        {
            ReputationVM.TestModels = ReputationVM.AllReputations.Where(x => x.Standing.Level == "Friendly").ToObservableCollection();

            OnPropertyChanged();
        });

        public ICommand ShowNeutralReputations => new RelayCommand<string>(mode =>
        {
            ReputationVM.TestModels = ReputationVM.AllReputations.Where(x => x.Standing.Level == "Neutral").ToObservableCollection();

            OnPropertyChanged();
        });

        public ICommand ShowUnfriendlyReputations => new RelayCommand<string>(mode =>
        {
            //var test = _windowFactory.GetUserControl<TestView>();
            //test.DataContext = _testVM;
            //CurrentView = _testVM;

            ReputationVM.TestModels = ReputationVM.AllReputations.Where(x => x.Standing.Level == "Unfriendly").ToObservableCollection();

            OnPropertyChanged();
        });

        public ICommand ShowHostileReputations => new RelayCommand<string>(mode =>
        {
            ReputationVM.TestModels = ReputationVM.AllReputations.Where(x => x.Standing.Level == "Hostile").ToObservableCollection();

            OnPropertyChanged();
        });

        public ICommand ShowHatedReputations => new RelayCommand<string>(mode =>
        {
            ReputationVM.TestModels = ReputationVM.AllReputations.Where(x => x.Standing.Level == "Hated").ToObservableCollection();

            OnPropertyChanged();
        });
    }
}
