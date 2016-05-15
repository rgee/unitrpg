using Contexts.Battle.Models;
using strange.extensions.signal.impl;

namespace Contexts.Battle.Signals {
    public class PhaseChangeCompleteSignal : Signal<BattlePhase> {
    }
}