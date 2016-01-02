using Combat;
using System.Collections.Generic;
using UnityEngine;

namespace Grid {
    public interface ITrigger {
        IScriptedEvent Event { get; }
    }
}