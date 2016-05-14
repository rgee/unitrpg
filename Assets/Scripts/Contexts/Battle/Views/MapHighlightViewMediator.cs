using System.Collections.Generic;
using Contexts.Battle.Models;
using Contexts.Battle.Signals;
using Contexts.Battle.Utilities;
using strange.extensions.mediation.impl;
using UnityEngine;

namespace Contexts.Battle.Views {
    public class MapHighlightViewMediator : Mediator {
        [Inject]
        public HoveredTileChangeSignal HoveredTileChangeSignal { get; set; }

        [Inject]
        public HoverTileDisableSignal HoverTileDisableSignal { get; set; }

        [Inject]
        public NewMapHighlightSignal MapHighlightSignal { get; set; }

        [Inject]
        public ClearHighlightSignal ClearHighlightSignal { get; set; }

        [Inject]
        public BattleViewState Model { get; set; }

        [Inject]
        public MapHighlightView View { get; set; }

        public override void OnRegister() {
            base.OnRegister();

            HoveredTileChangeSignal.AddListener(OnHighlightPositionChange);
            HoverTileDisableSignal.AddListener(OnHighlightDisable);
            ClearHighlightSignal.AddListener(OnClearHighlight);
            MapHighlightSignal.AddListener(OnHighlight);
        }

        private void OnHighlightPositionChange(Vector3 newPosition) {
           View.SetHighlightedPosition(newPosition); 
        }

        public void OnClearHighlight(HighlightLevel level) {
            View.ClearHighlightedPositions(level);
        }

        private void OnHighlight(MapHighlights highlights) {
            var dims = Model.Dimensions;
            View.HighlightPositions(highlights.Positions, highlights.Level, dims);
        }

        private void OnHighlightDisable() {
            View.DisableHoverHighlight();
        }
    }
}