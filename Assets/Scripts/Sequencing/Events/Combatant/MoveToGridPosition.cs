using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Contexts.Battle.Views;
using UnityEngine;
using Utils;
using WellFired;

namespace Assets.Sequencing.Events.Combatant {
    [USequencerFriendlyName("Move To Grid Position")]
    [USequencerEvent("Combatant/MoveToGridPosition")]        
    [USequencerEventHideDuration]
    [ExecuteInEditMode]
    public class MoveToGridPosition : USEventBase {
        public Vector2 Destination;

        private CombatantView _combatantView;
        private MapView _mapView;

        void Awake() {
            var mapObject = GameObject.FindGameObjectWithTag("Map");
            if (mapObject == null) {
                throw new ArgumentException("Grid movement sequence event cannot work without a MapView tagged 'Map'.");
            }

            _mapView = mapObject.GetComponent<MapView>();
        }


#if UNITY_EDITOR
        void Update() {
            _combatantView = AffectedObject.GetComponent<CombatantView>();
            var start = _mapView.GetDimensions().GetGridPositionForWorldPosition(AffectedObject.transform.position);
            var distance = MathUtils.ManhattanDistance(start, Destination);
            var secondsPerSquare = _combatantView.SecondsPerSquare; 
            Duration = distance*secondsPerSquare;
        }
#endif

        public override void FireEvent() {
            var view = AffectedObject.GetComponent<CombatantView>();
            var dimensions = _mapView.GetDimensions();
            var map = _mapView.Map;
            var gridStart = dimensions.GetGridPositionForWorldPosition(AffectedObject.transform.position);
            var path = map.FindPath(gridStart, Destination);
            if (path == null) {
                throw new ArgumentException("Couldn't find path.");
            }

            var combatant = _mapView.CombatantDatabase.GetCombatantById(view.CombatantId);
            _mapView.Map.MoveCombatant(combatant, Destination);
            _mapView.MoveUnit(view.CombatantId, path);
        }

        public override void ProcessEvent(float runningTime) {
        }
    }
}