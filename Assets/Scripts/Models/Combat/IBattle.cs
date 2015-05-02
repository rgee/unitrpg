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

        bool CanAct(Models.Combat.Unit unit);
        void WaitUnit(Models.Combat.Unit unit);
        void MoveUnit(Models.Combat.Unit unit, List<Vector2> path, Vector2 location);

        #endregion

        #region Unit Query Operations

        int GetRemainingMoves(Models.Combat.Unit unit);
        Models.Combat.Unit GetUnitByName(string name);
        Models.Combat.Unit GetUnitByLocation(Vector2 position);
        IEnumerable<Models.Combat.Unit> GetFriendlyUnits();
        IEnumerable<Models.Combat.Unit> GetEnemyUnits();

        #endregion

        #region Fight Operations

        Fight SimulateFight(Models.Combat.Unit attacker, AttackType attack, Models.Combat.Unit defender);
        void ExecuteFight(Fight fight);

        #endregion
    }
}