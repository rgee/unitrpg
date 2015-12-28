using System.Collections.Generic;
using UnityEngine;

namespace Models.Combat {
    public interface IMap {
        bool IsOccupied(Vector2 position);
        Unit GetUnitByPosition(Vector2 position);
        IEnumerable<Unit> GetAllUnits();
        IEnumerable<Unit> GetFriendlyUnits();
        IEnumerable<Unit> GetEnemyUnits();
        void MoveUnit(Unit unit, Vector2 location);
        void AddUnit(Unit unit);

        IEnumerable<InteractiveTile> GetAdjacentInteractiveTiles(Vector2 position);
        InteractiveTile GetTileByPosition(Vector2 position);
        void AddInteractiveTile(InteractiveTile tile);
        void RemoveInteractiveTile(Vector2 position);
    }
}