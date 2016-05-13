using System.Collections.Generic;
using Contexts.Battle.Signals;
using Contexts.Battle.Utilities;
using Models.Combat;
using Models.Fighting;
using Models.Fighting.Battle;
using Models.Fighting.Maps;
using Models.Fighting.Skills;
using UnityEngine;

namespace Contexts.Battle.Models {
    public class BattleViewState {
        private BattleUIState _state;

        public BattleUIState State {
            get { return _state; }
            set {
                var transition = new StateTransition(_state, value);
                StateTransitionSignal.Dispatch(transition);
                _state = value;
            }
        }

        public MovementPath CurrentMovementPath { get; set; }

        public IBattle Battle { get; set; }

        public IMap Map { get; set; }

        public ICombatant SelectedCombatant { get; set; }

        public SkillType SelectedSkillType { get; set; }

        public Vector2 HoveredTile { get; set; }

        public HashSet<CombatActionType> AvailableActions { get; set; }

        public MapDimensions Dimensions { get; set; }

        [Inject]
        public StateTransitionSignal StateTransitionSignal { get; set; }
    }
}