using Contexts.Battle.Models;
using Contexts.Battle.Signals.Camera;
using strange.extensions.command.impl;

namespace Contexts.Battle.Commands {
    public class RunEnemyActionsCommand : Command {
        
        [Inject]
        public BattleViewState BattleViewState { get; set; }

        [Inject]
        public CameraPanSignal CameraPanSignal { get; set; }

        public override void Execute() {
            var battle = BattleViewState.Battle;
            var actions = battle.ComputeEnemyActions();
        }
    }
}