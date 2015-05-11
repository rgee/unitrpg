using System.Collections.Generic;
using UnityEngine;

namespace Models.Combat {
    /// <summary>
    /// This is the main controller interface to the game logic. 
    /// All code on the surface of and beneath this interface should be ignorant of 
    /// any concept of a view or a screen or player input.
    /// </summary>
    public interface IBattle {
        #region Turn Operations

        bool IsComplete();
        bool IsFailed();
        void EndTurn();
        int TurnCount { get; }

        #endregion

        #region Unit Action Operations

        bool CanMove(Unit unit);
        bool CanAct(Unit unit);
        void WaitUnit(Unit unit);
        void MoveUnit(Unit unit, List<Vector2> path, Vector2 location);
        IEnumerable<CombatAction> GetAvailableActions(Unit unit); 

        #endregion

        #region Unit Query Operations

        int GetMovesUsed(Unit unit);
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