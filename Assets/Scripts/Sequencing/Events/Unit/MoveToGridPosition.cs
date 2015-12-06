using System.Collections;
using DG.Tweening;
using UnityEngine;
using WellFired;

namespace Assets.Sequencing.Events.Unit {
    [USequencerFriendlyName("Move To Grid Position")]
    [USequencerEvent("Unit/MoveToGridPosition")]        
    [USequencerEventHideDuration]
    [ExecuteInEditMode]
    public class MoveToGridPosition : USEventBase {
        public Vector2 Destination;
        public Vector2 Start;
        private Grid.Unit _unit;
        private MapGrid _grid;

        void Awake() {
            _grid = CombatObjects.GetMap();
        }

#if UNITY_EDITOR
        void Update() {
            _unit = AffectedObject.GetComponent<Grid.Unit>();
            var distance = MathUtils.ManhattanDistance(Start, Destination);
            var secondsPerSquare = _unit.model.Character.MoveTimePerSquare;
            Duration = distance*secondsPerSquare;
        }
#endif

        public override void FireEvent() {
            var distance = MathUtils.ManhattanDistance(Start, Destination);
            var worldDestinaion = _grid.GetWorldPosForGridPos(Destination);
            var worldStart = _grid.GetWorldPosForGridPos(Start);
            AffectedObject.transform.localPosition = worldStart;

            StartCoroutine(RunTo(worldDestinaion, Duration));
        }

        private IEnumerator RunTo(Vector3 worldDestination, float time) {
            var direction = MathUtils.DirectionTo(AffectedObject.transform.localPosition, worldDestination);
            _unit.Facing = direction;
            _unit.Running = true;
            yield return _unit.transform.DOLocalMove(worldDestination, time).SetEase(Ease.Linear).WaitForCompletion();
            _unit.Running = false;
            _grid.RescanGraph();
        }

        public override void ProcessEvent(float runningTime) {
        }
    }
}