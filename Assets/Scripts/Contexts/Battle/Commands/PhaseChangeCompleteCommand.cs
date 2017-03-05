using System;
using System.Linq;
using Contexts.Battle.Models;
using Contexts.Battle.Signals;
using strange.extensions.command.impl;
using UnityEngine;

namespace Contexts.Battle.Commands {
    public class PhaseChangeCompleteCommand : Command {
        [Inject]
        public BattleViewState Model { get; set; }

        [Inject]
        public BattlePhase Phase { get; set; }

        [Inject]
        public EventHandlersCompleteSignal EventHandlersCompleteSignal { get; set; }

        [Inject]
        public BattleEventRegistry BattleEventRegistry { get; set; }

        [Inject]
        public ProcessEventHandlersSignal ProcessEventHandlersSignal { get; set; }

        public override void Execute() {
            if (Model.Phase != BattlePhase.NotStarted) {
                Model.ResetUnitState();
                Model.Battle.EndTurn();
            }

            // If there are any events slated to happen at the start of this turn,
            // run them before releasing control back to the player.
            var turnEventHanders = Model.Battle.GetCurrentTurnEvents()
                .Select(eventName => BattleEventRegistry.GetHandler(eventName))
                .Where(x => x != null)
               .ToList();
            if (turnEventHanders.Count > 0) {
                Retain();
                Action action = null;
                action = new Action(() => {
                   StartNextTurn();
                   Release();
                });
                EventHandlersCompleteSignal.AddListener(action);
                ProcessEventHandlersSignal.Dispatch(turnEventHanders);
            } else {
                StartNextTurn();                
            }
        }

        void StartNextTurn() {
            if (Phase == BattlePhase.Enemy) {
                Model.State = BattleUIState.EnemyTurn;
            } else if (Phase == BattlePhase.Player) {
                Model.State = BattleUIState.SelectingUnit;
            }

            Model.Phase = Phase;
        }
    }
}
