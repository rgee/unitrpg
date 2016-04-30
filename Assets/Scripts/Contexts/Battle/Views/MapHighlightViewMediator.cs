using Contexts.Battle.Signals;
using strange.extensions.mediation.impl;
using UnityEngine;

namespace Contexts.Battle.Views {
    public class MapHighlightViewMediator : Mediator {
        [Inject]
        public HoveredTileChangeSignal HoveredTileChangeSignal { get; set; }

        [Inject]
        public MapHighlightView View { get; set; }

        public override void OnRegister() {
            base.OnRegister();

            HoveredTileChangeSignal.AddListener(OnHighlightPositionChange);
        }

        private void OnHighlightPositionChange(Vector3 newPosition) {
           View.SetHighlightedPosition(newPosition); 
        }
    }
}