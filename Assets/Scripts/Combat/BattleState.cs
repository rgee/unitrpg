using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (Objective))]
public class BattleState : MonoBehaviour {
    // Attack
    public Grid.Unit AttackTarget;
    public FightResult FightResult;
    // Movement
    public Vector2 MovementDestination;
    public Vector2 SelectedGridPosition;
    public Grid.Unit SelectedUnit;
    private Dictionary<Grid.Unit, UnitActionState> States = new Dictionary<Grid.Unit, UnitActionState>();

    public bool isWon() {
        return GetComponent<Objective>().IsComplete();
    }

    public bool isLost() {
        return GetComponent<Objective>().IsFailed();
    }

    public void ResetMovementState() {
        SelectedUnit = null;
        AttackTarget = null;
        FightResult = null;
        SelectedGridPosition = Vector2.zero;
        MovementDestination = Vector2.zero;
    }

    public void ResetTurnState() {
        States = new Dictionary<Grid.Unit, UnitActionState>();
    }

    public bool UnitActed(Grid.Unit unit) {
        if (!States.ContainsKey(unit)) {
            States[unit] = new UnitActionState();
        }

        return States[unit].Acted;
    }

    public bool UnitMoved(Grid.Unit unit) {
        if (!States.ContainsKey(unit)) {
            States[unit] = new UnitActionState();
        }

        var character = unit.GetCharacter();
        return States[unit].DistanceMoved >= character.Movement;
    }

    public int GetUsedDistance(Grid.Unit unit) {
        if (!States.ContainsKey(unit)) {
            States[unit] = new UnitActionState();
        }
        return States[unit].DistanceMoved;
    }

    public int GetRemainingDistance(Grid.Unit unit) {
        if (!States.ContainsKey(unit)) {
            States[unit] = new UnitActionState();
        }
        var character = unit.GetCharacter();
        return character.Movement - States[unit].DistanceMoved;
    }

    public void MarkUnitMoved(Grid.Unit unit, int distance) {
        if (!States.ContainsKey(unit)) {
            States[unit] = new UnitActionState();
        }
        var state = States[unit];
        state.DistanceMoved += distance;

        States[unit] = state;
    }

    public void MarkUnitActed(Grid.Unit unit) {
        if (!States.ContainsKey(unit)) {
            States[unit] = new UnitActionState();
        }
        var state = States[unit];
        state.Acted = true;

        States[unit] = state;
    }

    // Record of a unit's actions during a single turn
    private struct UnitActionState {
        public bool Acted;
        public int DistanceMoved;
    }
}