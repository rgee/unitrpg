using System.Collections;
using System.Collections.Generic;
using Combat;
using Contexts.Battle.Utilities;
using Models.Fighting.Characters;
using strange.extensions.mediation.impl;
using UnityEngine;

namespace Contexts.Battle.Views {
    [RequireComponent(typeof(FightPhaseAnimator))]
    [RequireComponent(typeof(CombatantController))]
    public class CombatantView : View {

        public string CharacterName;
        public string CombatantId;
        public ArmyType Army = ArmyType.Friendly;
        public CombatantState State = CombatantState.Idle;
        public MathUtils.CardinalDirection Facing = MathUtils.CardinalDirection.S;
        public float SecondsPerSquare = 0.3f;

        private FightPhaseAnimator _phaseAnimator;
        private CombatantController _controller;

        void Awake() {
            _phaseAnimator = GetComponent<FightPhaseAnimator>();
            _controller = GetComponent<CombatantController>();
        }


        public IEnumerator FollowPath(IList<Vector3> path, MapDimensions dimensions) {
            return _controller.FollowPath(path, dimensions);
        }
    }
}