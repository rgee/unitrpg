using Contexts.Battle.Models;
using Contexts.Battle.Signals;
using strange.extensions.command.impl;
using UnityEngine;

namespace Contexts.Battle.Commands {
    public class StartBattleCommand : Command {
        [Inject]
        public BattleViewState ViewState { get; set; }

        [Inject]
        public GatherBattleFromEditorSignal GatherSignal { get; set; }
        
        [Inject]
        public PhaseChangeStartSignal PhaseChangeStartSignal { get; set; }

        public override void Execute() {
            GatherSignal.Dispatch();
            ViewState.State = BattleUIState.PhaseChanging;
            PhaseChangeStartSignal.Dispatch(BattlePhase.Player);
        }
    }
}
