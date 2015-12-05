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
        public float SecondsPerSquare = 0.3f;
        public Vector2 Destination;
        public Vector2 Start;
        private Grid.Unit _unit;

        void Awake() {
            _unit = AffectedObject.GetComponent<Grid.Unit>();
        }

#if UNITY_EDITOR
        void Update() {
            var distance = MathUtils.ManhattanDistance(Start, Destination);
            Duration = distance*SecondsPerSquare;
        }
#endif

        public override void FireEvent() {

            var distance = MathUtils.ManhattanDistance(Start, Destination);
            var worldDestinaion = CombatObjects.GetMap().GetWorldPosForGridPos(Destination);

            StartCoroutine(RunTo(worldDestinaion, distance*SecondsPerSquare));
        }

        private IEnumerator RunTo(Vector3 worldDestination, float time) {
            
            var direction = MathUtils.DirectionTo(AffectedObject.transform.localPosition, worldDestination);
            _unit.Facing = direction;
            _unit.Running = true;
            yield return _unit.transform.DOLocalMove(worldDestination, time).SetEase(Ease.Linear).WaitForCompletion();
            _unit.Running = false;
        }

        public override void ProcessEvent(float runningTime) {
        }

        public override void EndEvent() {
            _unit.Running = false;
        }
    }
}