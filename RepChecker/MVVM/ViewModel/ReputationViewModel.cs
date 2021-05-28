using RepChecker.Core;
using RepChecker.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace RepChecker.MVVM.ViewModel
{
    public class ReputationViewModel : ViewModelBase
    {
        private ObservableCollection<ReputationModel> _testModels;

        private List<ReputationModel> _allReputations = new List<ReputationModel>()
        {
            new ReputationModel(),
            new ReputationModel(),
            new ReputationModel(),
            new ReputationModel(),
            new ReputationModel(),
            new ReputationModel(),
            new ReputationModel(),
            new ReputationModel(),
            new ReputationModel(),
            new ReputationModel() { Character = "Windmixdh", Realm = "Kazzak", ReputationName = "Unshackled",  Standing = new StandingModel 
            { 
                CurrentValue = 21000, Level = "Exalted", Max = 21000, Raw = 60000 } 
            },
            new ReputationModel(),
            new ReputationModel(),
            new ReputationModel(),
            new ReputationModel() { Character = "Windmixdh", Realm = "Kazzak", ReputationName = "Unshackled",  Standing = new StandingModel
            {
                CurrentValue = 21000, Level = "Exalted", Max = 21000, Raw = 60000 }
            },
            new ReputationModel() { Character = "Windmixhunt", Realm = "Kazzak", ReputationName = "Unshackled",  Standing = new StandingModel
            {
                CurrentValue = 21000, Level = "Exalted", Max = 21000, Raw = 60000 }
            },
            new ReputationModel() { Character = "Windmixlock", Realm = "Kazzak", ReputationName = "Unshackled",  Standing = new StandingModel
            {
                CurrentValue = 21000, Level = "Exalted", Max = 21000, Raw = 60000 }
            },
        };

        public ReputationModel Reputation { get; set; }

        private double _currentValue = 5500;
        private double _maxRepValue = 6000;

        public ReputationViewModel()
        {

        }


        public List<ReputationModel> AllReputations
        {
            get => _allReputations; 
            set 
            { 
                _allReputations = value;
            }
        }

        public ObservableCollection<ReputationModel> TestModels 
        {
            get => _testModels; 
            set
            {
                _testModels = value;
                OnPropertyChanged();
            }
        }


        public double CurrentRepValue
        {
            get => _currentValue;
            set
            {
                _currentValue = value;
                OnPropertyChanged(nameof(CurrentRepValue));
            }
        }

        public double MaxRepValue
        {
            get => _maxRepValue;
            set 
            { 
                _maxRepValue = value;
                OnPropertyChanged(nameof(MaxRepValue));
            }
        }

    }
}
