using UnityEngine;
using System.Collections;

public class Moving : StateMachineBehaviour {
    private BattleState State;
    private MapGrid Grid;
    private Grid.UnitManager UnitManager;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        State = GameObject.Find("BattleManager").GetComponent<BattleState>();
		Grid = GameObject.Find("Grid").GetComponent<MapGrid>();
        UnitManager = GameObject.Find("Unit Manager").GetComponent<Grid.UnitManager>();

        Grid.Unit unit = State.SelectedUnit;
        unit.MoveTo(State.MovementDestination, Grid, (arg) => { }, (arg) => {
            UnitManager.ChangeUnitPosition(State.SelectedUnit.gameObject, State.SelectedGridPosition);
            State.Reset();
            animator.SetTrigger("unit_moved");
        });
    }
}
