using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Contexts.Battle.Models;
using Contexts.Battle.Signals;
using strange.extensions.command.impl;
using UnityEngine;

namespace Contexts.Battle.Commands {
    public class ActionCompleteCommand : Command {
        [Inject]
        public BattleViewState Model { get; set; }

        [Inject]
        public NextBattleSignal NextBattleSignal { get; set; }

        [Inject]
        public IncrememtTurnSignal IncrememtTurnSignal { get; set; }

        [Inject]
        public ProcessTileEventsSignal ProcessTileEventsSignal { get; set; }
        
        [Inject]
        public BattleEventRegistry BattleEventRegistry { get; set; }

        public override void Execute() {
            // Flush the move action
            var battle = Model.Battle;
            if (Model.State == BattleUIState.CombatantMoving) {
                var path = Model.CurrentMovementPath.Positions.Skip(1).ToList();
                battle.MoveCombatant(Model.CurrentMovementPath.Combatant, path);
            } else if (Model.State == BattleUIState.Fighting) {
                battle.ExecuteFight(Model.FinalizedFight);
            }

            if (Model.State == BattleUIState.Uninitialized) {
                return;
            }

            if (battle.IsWon()) {
                Debug.Log("The battle is won.");
                NextBattleSignal.Dispatch();
                return;
            } else if (battle.IsLost()) {
                Debug.Log("The battle is lost.");
                return;
            }

            // If there are events to process because units walked over event tiles
            // handle them first.
            if (Model.EventsThisActionPhase.Count > 0) {
                var events = Model.EventsThisActionPhase;
                var handlers = events.Select(evt => BattleEventRegistry.GetHandler(evt)).ToList();
                ProcessTileEventsSignal.Dispatch(handlers);
            } else {
                IncrememtTurnSignal.Dispatch();                                
            }
        }
    }
}
