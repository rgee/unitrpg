using Models.Combat;
using strange.extensions.signal.impl;

namespace Contexts.Battle.Signals {
    /// <summary>
    /// Dispatched whenever the user selects a combat action to use.
    /// </summary>
    public class CombatActionSelectedSignal : Signal<CombatActionType> {
    }
}