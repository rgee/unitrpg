using UnityEngine;
using System.Collections.Generic;
using Models.Fighting.Maps.Triggers;
using strange.extensions.signal.impl;

namespace Models.Fighting.Maps {
    public interface IMap {
        Signal<EventTile> EventTileTriggeredSignal { get; }

        void AddCombatant(ICombatant combatant);

        void RemoveCombatant(ICombatant combatant);

        void MoveCombatant(ICombatant combatant, Vector2 position);

        List<ICombatant> GetAllOnMap(); 

        HashSet<ICombatant> GetAdjacent(Vector2 position);

        ICombatant GetAtPosition(Vector2 position);



        bool IsBlocked(Vector2 position);

        bool IsBlockedByEnvironment(Vector2 position);

        void AddObstruction(Vector2 position);



        void AddEventTile(EventTile eventTile);

        void RemoveEventTile(Vector2 location);

        EventTile GetEventTile(Vector2 location);

        void TriggerEventTile(Vector2 location);



        HashSet<Vector2> BreadthFirstSearch(Vector2 start, int maxDistance, bool ignoreOtherUnits);

        List<Vector2> FindPath(Vector2 start, Vector2 goal);
    }
}