using System;
using System.Collections.Generic;
using System.Text;

namespace RepChecker.MVVM.Model
{
    public class ApplicationUserModel
    {
        public string BattleTag { get; set; }
        public int Id { get; set; }
        public string LastUpdate { get; set; }

        public List<ReputationModel> UserReputations { get; set; }
    }
}
