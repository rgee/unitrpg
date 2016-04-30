using Contexts.Battle.Signals;
using Contexts.Battle.Utilities;
using Models.Fighting;
using strange.extensions.mediation.impl;
using UnityEngine;

namespace Contexts.Battle.Views {
    public class MapViewMediator : Mediator {
        [Inject]
        public MapView View { get; set; }

        [Inject]
        public MapPositionClickedSignal MapPositionClickedSignal { get; set; }

        [Inject]
        public InitializeMapSignal InitializeMapSignal { get; set; }

        public override void OnRegister() {
            View.MapClicked.AddListener(OnMapClicked);

            var dimensions = new MapDimensions(View.Width, View.Height);
            var combatants = View.GetCombatants();
            var randomizer = new BasicRandomizer();
            var config = new MapConfiguration(dimensions, combatants, randomizer);
            InitializeMapSignal.Dispatch(config);
        }

        private void OnMapClicked(Vector2 clickPosition) {
            MapPositionClickedSignal.Dispatch(clickPosition);
        }
    }
}