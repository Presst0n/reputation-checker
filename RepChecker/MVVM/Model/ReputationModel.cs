using System;
using System.Collections.Generic;
using System.Text;

namespace RepChecker.MVVM.Model
{
    public class ReputationModel
    {
        public string ReputationName { get; set; } = "Undercity";

        public string Character { get; set; } = "Windiwitch";

        public string Realm { get; set; } = "Kazzak";

        public StandingModel Standing { get; set; } = new StandingModel();

        private string _repItemColor;

    }
}
