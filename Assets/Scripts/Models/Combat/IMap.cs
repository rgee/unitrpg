
using System.Collections.Generic;
using UnityEngine;

namespace Models.Combat {
    public interface IMap {
        Unit GetUnitByPosition(Vector2 position);
        IEnumerable<Unit> GetAllUnits(); 
        IEnumerable<Unit> GetFriendlyUnits();
        IEnumerable<Unit> GetEnemyUnits();
        void MoveUnit(Unit unit, Vector2 location);
    }
}