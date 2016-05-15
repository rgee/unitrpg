using UnityEngine;
using System.Collections.Generic;

namespace Models.Fighting.Maps {
    public interface IMap {
        void AddCombatant(ICombatant combatant);

        void RemoveCombatant(ICombatant combatant);

        void MoveCombatant(ICombatant combatant, Vector2 position);

        List<ICombatant> GetAllOnMap(); 

        bool IsBlocked(Vector2 position);

        bool IsBlockedByEnvironment(Vector2 position);

        void AddObstruction(Vector2 position);

        HashSet<ICombatant> GetAdjacent(Vector2 position);

        ICombatant GetAtPosition(Vector2 position);
        
        HashSet<Vector2> BreadthFirstSearch(Vector2 start, int maxDistance, bool ignoreOtherUnits);

        List<Vector2> FindPath(Vector2 start, Vector2 goal);
    }
}