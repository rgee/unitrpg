using System.Collections.Generic;
using Models.Combat;
using Models.Combat.Inventory;
using UnityEngine;

public class BattleState : MonoBehaviour {
    public IBattle Model;

    // Attack
    public Grid.Unit AttackTarget;
    public FightResult FightResult;
    // Movement
    public Vector2 MovementDestination;
    public Vector2 SelectedGridPosition;
    public Grid.Unit SelectedUnit;
    // Items
    public Item SelectedItem;

    public bool isWon() {
        return CombatObjects.GetObjective().IsComplete();
    }

    public bool isLost() {
        return CombatObjects.GetObjective().IsFailed();
    }

    public void ResetToUnitSelectedState() {
        AttackTarget = null;
        FightResult = null;
        MovementDestination = Vector2.zero;
    }

    public void ResetMovementState() {
        SelectedUnit = null;
        AttackTarget = null;
        FightResult = null;
        SelectedGridPosition = Vector2.zero;
        MovementDestination = Vector2.zero;
    }

    public void ResetTurnState() {

    }

    public bool UnitActed(Grid.Unit unit) {
        return !Model.CanAct(unit.model);
    }

    public bool UnitMoved(Grid.Unit unit) {
        return !Model.CanMove(unit.model);
    }

    public int GetUsedDistance(Grid.Unit unit) {
        return Model.GetMovesUsed(unit.model);
    }

    public int GetRemainingDistance(Grid.Unit unit) {
        return Model.GetRemainingMoves(unit.model);
    }
}