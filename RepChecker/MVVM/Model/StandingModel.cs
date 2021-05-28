namespace RepChecker.MVVM.Model
{
    public class StandingModel
    {
        public uint Raw { get; set; } = 15000;
        public uint Max { get; set; } = 12000;
        public uint CurrentValue { get; set; } = 6200;
        //public uint Tier { get; set; }
        public string Level { get; set; } = "Revered";
    }
}