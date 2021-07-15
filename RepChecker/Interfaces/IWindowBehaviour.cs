using System;

namespace RepChecker.Interfaces
{
    public interface IWindowBehaviour
    {
        Action Close { get; set; }
        Action Minimize { get; set; }
    }
}
