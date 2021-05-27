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
            new ReputationModel { Title = "Exalted" },
            new ReputationModel { Title = "Exalted" },
            new ReputationModel { Title = "Exalted" },
            new ReputationModel { Title = "Exalted" },
            new ReputationModel { Title = "Exalted" },
            new ReputationModel { Title = "Hated" },
            new ReputationModel { Title = "Hated" },
            new ReputationModel { Title = "Hated" },
            new ReputationModel { Title = "Hated" },
            new ReputationModel { Title = "Hated" },
            new ReputationModel { Title = "Hated" },
            new ReputationModel { Title = "Hated" },
            new ReputationModel { Title = "Hated" },
            new ReputationModel { Title = "Hated" },
            new ReputationModel { Title = "Hated" },
            new ReputationModel { Title = "Hated" },
            new ReputationModel { Title = "Hated" },
            new ReputationModel { Title = "Exalted" },
            new ReputationModel { Title = "Exalted" },
            new ReputationModel { Title = "Exalted" },
            new ReputationModel { Title = "Friendly" },
            new ReputationModel { Title = "Exalted" },
            new ReputationModel { Title = "Exalted" },
            new ReputationModel { Title = "Exalted" },
            new ReputationModel { Title = "Exalted" },
            new ReputationModel { Title = "Revered" },
            new ReputationModel { Title = "Revered" },
            new ReputationModel { Title = "Revered" },
            new ReputationModel { Title = "Revered" },
            new ReputationModel { Title = "Revered" },
            new ReputationModel { Title = "Revered" },
            new ReputationModel { Title = "Revered" },
            new ReputationModel { Title = "Revered" },
            new ReputationModel { Title = "Exalted" },
            new ReputationModel { Title = "Exalted" },
            new ReputationModel { Title = "Exalted" },
            new ReputationModel { Title = "Friendly" },
            new ReputationModel { Title = "Exalted" },
            new ReputationModel { Title = "Exalted" },
            new ReputationModel { Title = "Neutral" },
            new ReputationModel { Title = "Exalted" },
            new ReputationModel { Title = "Neutral" }
        };

        private double _currentValue = 2000;
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


        //private string colorState = "Transparent";
        //public string ColorState
        //{
        //    get { return colorState; }
        //    set 
        //    { 
        //        colorState = value; 
        //        OnPropertyChanged("ColorState"); 
        //    }
        //}

    }
}
