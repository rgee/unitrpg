using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Contexts.Battle.Models;
using Contexts.Battle.Signals;
using Models.Fighting.Battle;
using strange.extensions.command.impl;
using UnityEngine;

namespace Contexts.Battle.Commands {
    public class ActionCompleteCommand : Command {
        [Inject]
        public ICombatAction Action { get; set; }

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
            if (Model.State == BattleUIState.Uninitialized) {
                return;
            }

            var battle = Model.Battle;
            battle.SubmitAction(Action);

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
