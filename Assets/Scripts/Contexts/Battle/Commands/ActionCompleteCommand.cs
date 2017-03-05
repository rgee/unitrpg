using System;
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
        public ProcessEventHandlersSignal ProcessEventHandlersSignal { get; set; }

        [Inject]
        public EventHandlersCompleteSignal EventHandlersCompleteSignal { get; set; }
        
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

            var events = Model.EventsThisActionPhase;
            var handlers = events
                .Select(evt => BattleEventRegistry.GetHandler(evt))
                .Where(x => x != null)
                .ToList();
            // If there are events to process because units walked over event tiles
            // handle them first.
            if (handlers.Count > 0) {
                Retain();
                Action action = null;
                action = new Action(() => {
                    IncrememtTurnSignal.Dispatch();
                    Release();
                    EventHandlersCompleteSignal.RemoveListener(action);
                });
                EventHandlersCompleteSignal.AddListener(action);
                ProcessEventHandlersSignal.Dispatch(handlers);
            } else {
                IncrememtTurnSignal.Dispatch();                                
            }
        }
    }
}
