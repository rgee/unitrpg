using Models.Fighting.Battle;
using strange.extensions.signal.impl;

namespace Contexts.Battle.Signals {
    public class ActionCompleteSignal : Signal<ICombatAction> {
    }
}
