namespace RepChecker.MVVM.Model
{
    public class StandingModel
    {
        public int StandingId { get; set; }
        public int Raw { get; set; }
        public int Max { get; set; }
        public int CurrentValue { get; set; }
        public int Tier { get; set; }
        public string Level { get; set; }
        public int ReputationId { get; set; }
    }
}