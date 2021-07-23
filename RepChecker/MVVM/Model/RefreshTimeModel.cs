using System;
using System.Collections.Generic;
using System.Text;

namespace RepChecker.MVVM.Model
{
    public class RefreshTimeModel
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public TimeSpan RefreshmentTime { get; set; }
    }
}
