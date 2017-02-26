using System;
using System.Collections.Generic;
using System.Linq;
using Contexts.Battle.Models;
using Contexts.Battle.Signals;
using Contexts.Battle.Signals.Camera;
using Models.Fighting.AI;
using Models.Fighting.Battle;
using Models.Fighting.Characters;
using strange.extensions.command.impl;

namespace Contexts.Battle.Commands {
    public class RunEnemyActionsCommand : Command {

        [Inject]
        public EnemyTurnCompleteSignal EnemyTurnCompleteSignal { get; set; }
        
        [Inject]
        public BattleViewState BattleViewState { get; set; }

        [Inject]
        public CameraPanCompleteSignal CameraPanCompleteSignal { get; set; }

        [Inject]
        public ActionCompleteSignal ActionCompleteSignal { get; set; }

        [Inject]
        public AnimateActionSignal AnimateActionSignal { get; set; }

        [Inject]
        public CameraPanToPointOfInterestSignal PanToPointOfInterestSignal { get; set; }

        public override void Execute() {
            var battle = BattleViewState.Battle;
            var plan = battle.GetActionPlan(ArmyType.Enemy);

            if (plan.HasActionsRemaining()) {
                Retain();
                ProcessActions(plan.NextActionStep(battle), plan);
            } else {
                EnemyTurnCompleteSignal.Dispatch();
            }
        }

        private void ProcessActions(List<ICombatAction> actions, IActionPlan plan) {
            if (actions.Count > 0) {
                var nextAction = actions[0];
                var poiFactory = new PointOfInterestFactory(BattleViewState.Dimensions);
                var poi = nextAction.GetPointofInterest(poiFactory);

                Action panListener = null;
                panListener = new Action(() => {
                    
                    Action listener = null;
                    listener = new Action(() => {
                        ProcessActions(actions.Skip(1).ToList(), plan);
                        ActionCompleteSignal.RemoveListener(listener);
                    });
                    ActionCompleteSignal.AddListener(listener);
                    AnimateActionSignal.Dispatch(nextAction);

                    CameraPanCompleteSignal.RemoveListener(panListener);
                });
                CameraPanCompleteSignal.AddListener(panListener);
                PanToPointOfInterestSignal.Dispatch(poi);
            } else if (plan.HasActionsRemaining()) {
                ProcessActions(plan.NextActionStep(BattleViewState.Battle), plan);
            } else {
                Release();
                EnemyTurnCompleteSignal.Dispatch();
            }
        }
    }
}