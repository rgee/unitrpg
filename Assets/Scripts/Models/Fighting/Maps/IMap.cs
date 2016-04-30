using UnityEngine;

namespace Models.Fighting.Maps {
    public interface IMap {
        void AddCombatant(ICombatant combatant);

        void MoveCombatant(ICombatant combatant, Vector2 position);

        bool IsBlocked(Vector2 position);

        bool IsBlockedByEnvironment(Vector2 position);

        void AddObstruction(Vector2 position);

        ICombatant GetAtPosition(Vector2 position);
    }
}