using System;
using System.Collections.Generic;
using System.Text;

namespace RepChecker.MVVM.Model
{
    public class Reputations
    {
        public List<ReputationModel> Exalted { get; set; } 
        public List<ReputationModel> Revered { get; set; } 
        public List<ReputationModel> Honored { get; set; }
        public List<ReputationModel> Friendly { get; set; }
        public List<ReputationModel> Neutral { get; set; } 
        public List<ReputationModel> Unfriendly { get; set; }
        public List<ReputationModel> Hostile { get; set; } 
        public List<ReputationModel> Hated { get; set; }
    }
}
