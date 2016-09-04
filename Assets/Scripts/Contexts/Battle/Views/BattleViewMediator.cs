using System.Collections;
using System.Collections.Generic;
using Contexts.Battle.Models;
using Contexts.Battle.Signals;
using strange.extensions.mediation.impl;
using UnityEngine;

namespace Contexts.Battle.Views {
    public class BattleViewMediator : Mediator {
        [Inject]
        public BackSignal BackSignal { get; set; }

        [Inject]
        public IncrememtTurnSignal IncrememtTurnSignal { get; set; }

        [Inject]
        public BattleView View { get; set; }

        [Inject]
        public BattleViewState Model { get; set; }

        [Inject]
        public ProcessTileEventsSignal ProcessTileEventsSignal { get; set; } 

        public override void OnRegister() {
            View.TriggerEventsCompleteSignal.AddListener(OnEventTileProcessingComplete);
            ProcessTileEventsSignal.AddListener(ProcessTileEvents);
        }

        private void OnEventTileProcessingComplete() {
            IncrememtTurnSignal.Dispatch(); 
        }

        private void ProcessTileEvents(IEnumerable<IEnumerator> events) {
            View.TriggerEventHandlers(events);
        }

        void Update() {
            if (Model.State == BattleUIState.Uninitialized) {
                return;
            }
            if (Input.GetKeyDown(KeyCode.Escape)) {
                BackSignal.Dispatch();
            }            
        }
    }
}