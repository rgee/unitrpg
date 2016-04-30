using Contexts.Battle.Signals;
using strange.extensions.mediation.impl;
using UnityEngine;

namespace Contexts.Battle.Views {
    public class MapViewMediator : Mediator {
        [Inject]
        public MapView View { get; set; }

        [Inject]
        public MapPositionClicked MapPositionClicked { get; set; }

        public override void OnRegister() {
            View.MapClicked.AddListener(OnMapClicked);
        }

        private void OnMapClicked(Vector2 clickPosition) {
            MapPositionClicked.Dispatch(clickPosition);
        }
    }
}