﻿using System.Collections.Generic;
using Models.Combat;
using UnityEngine;

[RequireComponent(typeof (Objective))]
public class BattleState : MonoBehaviour {
    public IBattle Model;

    // Attack
    public Grid.Unit AttackTarget;
    public FightResult FightResult;
    // Movement
    public Vector2 MovementDestination;
    public Vector2 SelectedGridPosition;
    public Grid.Unit SelectedUnit;

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