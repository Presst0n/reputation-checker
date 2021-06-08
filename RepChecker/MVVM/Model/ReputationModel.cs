using System;
using System.Collections.Generic;
using System.Text;

namespace RepChecker.MVVM.Model
{
    public class ReputationModel
    {
        public uint Id { get; set; }
        public string ReputationName { get; set; }
        public string Character { get; set; }
        public string Realm { get; set; }
        public StandingModel Standing { get; set; }

    }
}
