using UnityEngine;

namespace Models.Fighting.Maps {
    public interface IMap {
        void AddCombatant(ICombatant combatant);
        void MoveCombatant(ICombatant combatant, Vector2 position);
    }
}