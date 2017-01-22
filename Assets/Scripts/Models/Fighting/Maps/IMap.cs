using System;
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


        HashSet<Vector2> RangeQuery(Vector2 center, int distance);

        HashSet<Vector2> RangeQuery(Vector2 center, int distance, Predicate<KeyValuePair<Vector2, Tile>> predicate);

        HashSet<Vector2> FindSurroundingPoints(Vector2 center, int distance);

        HashSet<Vector2> BreadthFirstSearch(Vector2 start, int maxDistance, bool ignoreOtherUnits);

        HashSet<Vector2> FindUnoccupiedSurroundingPoints(Vector2 start, int distance);

        List<Vector2> FindPathToAdjacentTile(Vector2 start, Vector2 goal);
        
        List<Vector2> FindPath(Vector2 start, Vector2 goal);
    }
}