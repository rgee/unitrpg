using Contexts.Battle.Signals;
using strange.extensions.mediation.impl;
using UnityEngine;

namespace Contexts.Battle.Views {
    public class BattleViewMediator : Mediator {
        [Inject]
        public BackSignal BackSignal { get; set; }

        void Update() {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                BackSignal.Dispatch();
            }            
        } 
    }
}