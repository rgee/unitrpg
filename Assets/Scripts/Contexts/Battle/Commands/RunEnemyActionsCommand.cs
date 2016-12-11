using Contexts.Battle.Models;
using Contexts.Battle.Signals;
using Contexts.Battle.Signals.Camera;
using strange.extensions.command.impl;

namespace Contexts.Battle.Commands {
    public class RunEnemyActionsCommand : Command {

        [Inject]
        public EnemyTurnCompleteSignal EnemyTurnCompleteSignal { get; set; }
        
        [Inject]
        public BattleViewState BattleViewState { get; set; }

        [Inject]
        public CameraPanSignal CameraPanSignal { get; set; }

        [Inject]
        public CameraPanCompleteSignal CameraPanCompleteSignal { get; set; }

        [Inject]
        public ActionCompleteSignal ActiuonCompleteSignal { get; set; }

        public override void Execute() {
            var battle = BattleViewState.Battle;
            var actions = battle.ComputeEnemyActions();
            

            EnemyTurnCompleteSignal.Dispatch();
        }
    }
}