using System;
using Models.Fighting.Characters;

namespace Contexts.Battle.Models {
    public enum BattlePhase {
        NotStarted,
        Player,
        Enemy,
        Other
    }

    static class BattlePhaseMethods {
        public static ArmyType GetArmyType(this BattlePhase phase) {
            switch (phase) {
                case BattlePhase.Player:
                    return ArmyType.Friendly;
                case BattlePhase.Enemy:
                    return ArmyType.Enemy;
                case BattlePhase.Other:
                    return ArmyType.Other;
                default:
                    throw new ArgumentOutOfRangeException("phase", phase, null);
            }
        }
    }
}