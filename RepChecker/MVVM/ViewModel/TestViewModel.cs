using RepChecker.Core;
using RepChecker.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace RepChecker.MVVM.ViewModel
{
    public class TestViewModel : ViewModelBase
    {
        public ObservableCollection<ReputationModel> TestCollection { get; set; } = new ObservableCollection<ReputationModel>()
        {
            new ReputationModel { ReputationName = "Chlebaczek" },
            new ReputationModel { ReputationName = "Chlebaczek" },
            new ReputationModel { ReputationName = "Chlebaczek" },
            new ReputationModel { ReputationName = "Chlebaczek" },
            new ReputationModel { ReputationName = "Chlebaczek" }
        };
    }
}
