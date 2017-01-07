using System.Collections.Generic;
using Contexts.Battle.Models;
using Contexts.Battle.Signals;
using Contexts.Battle.Signals.Camera;
using Models.Fighting.Battle;
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

        [Inject]
        public AnimateActionSignal AnimateActionSignal { get; set; }

        public override void Execute() {
            var battle = BattleViewState.Battle;
            var actions = battle.ComputeEnemyActions();

            if (actions.Count > 0) {
                Retain();
                ProcessActions(new Stack<ICombatAction>(actions));
            } else {
                EnemyTurnCompleteSignal.Dispatch();
            }
        }

        private void ProcessActions(Stack<ICombatAction> actions) {
            var currentAction = actions.Pop();
            ActiuonCompleteSignal.AddOnce(action => {
                if (actions.Count == 0) {
                    Release();
                    EnemyTurnCompleteSignal.Dispatch();
                } else {
                    ProcessActions(actions);
                }
            });
            AnimateActionSignal.Dispatch(currentAction);
        }
    }
}