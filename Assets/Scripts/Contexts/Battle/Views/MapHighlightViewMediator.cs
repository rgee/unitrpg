using Contexts.Battle.Models;
using Contexts.Battle.Signals;
using Models.Fighting;
using strange.extensions.mediation.impl;
using UnityEngine;

namespace Contexts.Battle.Views {
    public class MapHighlightViewMediator : Mediator {
        [Inject]
        public HoveredTileChangeSignal HoveredTileChangeSignal { get; set; }

        [Inject]
        public HoverTileDisableSignal HoverTileDisableSignal { get; set; }

        [Inject]
        public MoveSelectedSignal MoveSelectedSignal { get; set; }

        [Inject]
        public BattleViewState Model { get; set; }

        [Inject]
        public MapHighlightView View { get; set; }

        public override void OnRegister() {
            base.OnRegister();

            HoveredTileChangeSignal.AddListener(OnHighlightPositionChange);
            HoverTileDisableSignal.AddListener(OnHighlightDisable);
            MoveSelectedSignal.AddListener(OnMoveSelected);
        }

        private void OnHighlightPositionChange(Vector3 newPosition) {
           View.SetHighlightedPosition(newPosition); 
        }

        private void OnMoveSelected() {
            var unit = Model.SelectedCombatant;
            var moveRange = unit.GetAttribute(Attribute.AttributeType.Move);
            var highlights = Model.Map.BreadthFirstSearch(unit.Position, moveRange.Value, false);
            var dims = Model.Dimensions;

            View.HighlightPositions(highlights, HighlightLevel.PlayerMove, dims);
        }

        private void OnHighlightDisable() {
            View.DisableHoverHighlight();
        }
    }
}