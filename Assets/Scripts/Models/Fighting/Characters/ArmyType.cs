using System;
using Contexts.Battle.Models;

namespace Models.Fighting.Characters {
    public enum ArmyType {
        Friendly,
        Enemy,
        Other
    }

    internal static class ArmyTypeMethods {
        public static BattlePhase GetBattlePhase(this ArmyType army) {
            switch (army) {
                case ArmyType.Friendly:
                    return BattlePhase.Player;
                case ArmyType.Enemy:
                    return BattlePhase.Enemy;
                case ArmyType.Other:
                    return BattlePhase.Other;
                default:
                    throw new ArgumentOutOfRangeException("army", army, null);
            }
        }
    }
}