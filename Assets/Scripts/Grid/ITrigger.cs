using Combat;
using UnityEngine;

namespace Grid {
    public interface ITrigger {
        IScriptedEvent Event { get; }
        Vector2 Location { get; }
    }
}