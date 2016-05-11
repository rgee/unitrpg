using System.Collections.Generic;
using Contexts.Battle.Utilities;
using Models.Combat;
using Models.Fighting;
using Models.Fighting.Battle;
using Models.Fighting.Maps;
using Models.Fighting.Skills;
using UnityEngine;

namespace Contexts.Battle.Models {
    public class BattleViewState {
        public BattleUIState State { get; set; }

        public IBattle Battle { get; set; }

        public IMap Map { get; set; }

        public ICombatant SelectedCombatant { get; set; }

        public SkillType SelectedSkillType { get; set; }

        public Vector2 HoveredTile { get; set; }

        public HashSet<CombatActionType> AvailableActions { get; set; }

        public MapDimensions Dimensions { get; set; }
    }
}