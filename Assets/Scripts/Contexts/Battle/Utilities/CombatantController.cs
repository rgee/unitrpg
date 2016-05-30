using System.Collections;
using System.Collections.Generic;
using Contexts.Battle.Views;
using DG.Tweening;
using strange.extensions.signal.impl;
using UnityEngine;
using Utils;

namespace Contexts.Battle.Utilities {
    [RequireComponent(typeof(CombatantView))]
    public class CombatantController : MonoBehaviour {
        public Signal<Vector2> SquareReachedSignal = new Signal<Vector2>();
        private CombatantView _view;

        void Awake() {
            _view = GetComponent<CombatantView>();
        }

        public IEnumerator FollowPath(IList<Vector3> path, MapDimensions dimensions) {
            
            var pathIndex = 0;
            var previousPoint = MathUtils.Round(transform.position);

            _view.State = CombatantState.Running;
            while (pathIndex < path.Count) {
                var currentDestination = MathUtils.Round(path[pathIndex]);
                _view.Facing = MathUtils.DirectionTo(previousPoint, currentDestination);

                var secondsPerSquare = _view.SecondsPerSquare;
                yield return transform
                    .DOMove(currentDestination, secondsPerSquare)
                    .SetEase(Ease.Linear)
                    .WaitForCompletion();

                var mapPosition = dimensions.GetGridPositionForWorldPosition(currentDestination);
                SquareReachedSignal.Dispatch(mapPosition);

                pathIndex++;
                previousPoint = currentDestination;
            }
            _view.State = CombatantState.Idle;
        }
    }
}