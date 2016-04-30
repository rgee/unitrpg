using Contexts.Battle.Models;
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
        public BattleViewState BattleModel { get; set; }

        [Inject]
        public MapPositionClickedSignal MapPositionClickedSignal { get; set; }

        [Inject]
        public InitializeMapSignal InitializeMapSignal { get; set; }

        [Inject]
        public GatherBattleFromEditorSignal GatherSignal { get; set; }

        [Inject]
        public HoverPositionSignal HoverPositionSignal { get; set; }

        public override void OnRegister() {
            View.MapClicked.AddListener(OnMapClicked);
            View.MapHovered.AddListener(OnMapHovered);

            GatherSignal.AddOnce(() => {
                var dimensions = new MapDimensions(View.Width, View.Height);
                var combatants = View.GetCombatants();
                var randomizer = new BasicRandomizer();
                var config = new MapConfiguration(dimensions, combatants, randomizer);
                InitializeMapSignal.Dispatch(config);
            });
        }

        private void OnMapHovered(Vector2 hoverPosition) {
            if (BattleModel.HoveredTile != hoverPosition) {
                var worldPosition = View.GetWorldPositionForGridPosition(hoverPosition);
                HoverPositionSignal.Dispatch(new GridPosition(hoverPosition, worldPosition));
            }
        }

        private void OnMapClicked(Vector2 clickPosition) {
            MapPositionClickedSignal.Dispatch(clickPosition);
        }
    }
}