using System.Collections.Generic;
using UnityEngine;

namespace Models.Combat {
    public interface IBattle {
        #region Turn Operations

        bool IsComplete();
        bool IsFailed();
        int TurnCount { get; }

        #endregion

        #region Unit Action Operations

        bool CanAct(Unit unit);
        void WaitUnit(Unit unit);
        void MoveUnit(Unit unit, List<Vector2> path, Vector2 location);

        #endregion

        #region Unit Query Operations

        int GetRemainingMoves(Unit unit);
        Unit GetUnitByName(string name);
        Unit GetUnitByLocation(Vector2 position);
        IEnumerable<Unit> GetFriendlyUnits();
        IEnumerable<Unit> GetEnemyUnits();

        #endregion

        #region Fight Operations

        Fight SimulateFight(Unit attacker, AttackType attack, Unit defender);
        void ExecuteFight(Fight fight);

        #endregion
    }
}