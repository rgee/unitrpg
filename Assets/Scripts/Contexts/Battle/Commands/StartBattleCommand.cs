using Contexts.Battle.Models;
using Contexts.Battle.Signals;
using Contexts.BattlePrep.Signals;
using Contexts.BattlePrep.Signals.Public;
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

        [Inject]
        public ClosePrepSignal ClosePrepSignal { get; set; }

        public override void Execute() {
            ClosePrepSignal.Dispatch();
            GatherSignal.Dispatch();
            ViewState.State = BattleUIState.PhaseChanging;
            PhaseChangeStartSignal.Dispatch(BattlePhase.Player);
        }
    }
}
