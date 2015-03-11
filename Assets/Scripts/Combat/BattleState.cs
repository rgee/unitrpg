using UnityEngine;
using System.Collections;

public class BattleState : MonoBehaviour {
	public Grid.Unit SelectedUnit;
	public Vector2 SelectedGridPosition;
	public Vector2 MovementDestination;

    public void Reset() {
        SelectedUnit = null;
        SelectedGridPosition = Vector2.zero;
        MovementDestination = Vector2.zero;
    }
}
