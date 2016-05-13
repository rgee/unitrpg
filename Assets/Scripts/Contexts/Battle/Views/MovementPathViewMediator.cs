using System.Collections.Generic;
using System.Linq;
using Contexts.Battle.Models;
using Contexts.Battle.Signals;
using strange.extensions.mediation.impl;
using UnityEngine;

namespace Contexts.Battle.Views {
    public class MovementPathViewMediator : Mediator {
        [Inject] 
        public MovementPathView View { get; set; }

        [Inject]
        public BattleViewState Model { get; set; }

        [Inject]
        public MovementPathReadySignal MovePathReadySignal { get; set; }

        [Inject]
        public MovementPathUnavailableSignal MovementPathUnavailableSignal { get; set; }

        public override void OnRegister() {
            base.OnRegister();
            MovementPathUnavailableSignal.AddListener(ClearPath);
            MovePathReadySignal.AddListener(ShowPath);
        }

        private void ClearPath() {
            View.ClearPath();
        }

        private void ShowPath(List<Vector2> path) {
            var worldPath = path
                .Select(tile => Model.Dimensions.GetWorldPositionForGridPosition(tile))
                .Select(pos => new Vector3(Mathf.Round(pos.x), Mathf.Round(pos.y), 0))
                .ToList();

            View.ShowPath(worldPath);
        } 
    }
}