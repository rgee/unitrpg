using UnityEngine;
using System.Collections;

public class Moving : StateMachineBehaviour {
    private BattleState State;
    private MapGrid Grid;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        State = GameObject.Find("BattleManager").GetComponent<BattleState>();
		Grid = GameObject.Find("Grid").GetComponent<MapGrid>();

        Grid.Unit unit = State.SelectedUnit;
        unit.MoveTo(State.MovementDestination, Grid, (arg) => { }, (arg) => {
            State.Reset();
            animator.SetTrigger("unit_moved");
        });
    }
}
