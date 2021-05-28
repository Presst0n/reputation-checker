﻿using RepChecker.Core;
using RepChecker.MVVM.Model;
using RepChecker.MVVM.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using RepChecker.Extensions;

namespace RepChecker.MVVM.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IWindowFactory _windowFactory;
        private readonly TestViewModel _testVM;
        private ViewModelBase _currentView = null;
        private bool _isUserLoggedIn = true;

        public MainViewModel(ReputationViewModel reputationVM, IWindowFactory windowFactory, TestViewModel testVM)
        {
            ReputationVM = reputationVM;
            //CurrentView = ReputationVM;
            _windowFactory = windowFactory;
            _testVM = testVM;
        }

        public ReputationViewModel ReputationVM { get; set; }

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
            CurrentView = ReputationVM;

            OnPropertyChanged();
        });

        public ICommand ShowExaltedReputations => new RelayCommand<string>(mode =>
        {
            ReputationVM.TestModels = ReputationVM.AllReputations.Where(x => x.Standing.Level == "Exalted").ToObservableCollection();

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
            //var test = _windowFactory.GetUserControl<TestView>();
            //test.DataContext = _testVM;
            //CurrentView = _testVM;

            ReputationVM.TestModels = ReputationVM.AllReputations.Where(x => x.Standing.Level == "Hostile").ToObservableCollection();

            OnPropertyChanged();
        });

        public ICommand ShowHatedReputations => new RelayCommand<string>(mode =>
        {
            //var test = _windowFactory.GetUserControl<TestView>();
            //test.DataContext = _testVM;
            //CurrentView = _testVM;

            ReputationVM.TestModels = ReputationVM.AllReputations.Where(x => x.Standing.Level == "Hated").ToObservableCollection();

            OnPropertyChanged();
        });
    }
}
