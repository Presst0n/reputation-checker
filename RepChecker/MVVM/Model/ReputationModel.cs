

namespace RepChecker.MVVM.Model
{
    public class ReputationModel
    {
        public int ReputationId { get; set; } // uint?
        public string ReputationName { get; set; }
        public string Character { get; set; }
        public string Realm { get; set; }
        public StandingModel Standing { get; set; }
        public string FactionHref { get; set; }
        public string BattleTag { get; set; }
    }
}
