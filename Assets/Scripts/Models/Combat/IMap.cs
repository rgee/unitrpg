
using System.Collections.Generic;
using UnityEngine;

namespace Models.Combat {
    public interface IMap {
        Models.Combat.Unit GetUnitByPosition(Vector2 position);
        IEnumerable<Models.Combat.Unit> GetAllUnits(); 
        IEnumerable<Models.Combat.Unit> GetFriendlyUnits();
        IEnumerable<Models.Combat.Unit> GetEnemyUnits();
        void MoveUnit(Models.Combat.Unit unit, Vector2 location);
    }
}