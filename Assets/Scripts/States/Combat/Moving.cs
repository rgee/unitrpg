using Grid;
using UnityEngine;

public class Moving : StateMachineBehaviour {
    private MapGrid Grid;
    private BattleState State;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        State = CombatObjects.GetBattleState();
        Grid = CombatObjects.GetMap();

        var unit = State.SelectedUnit;
        unit.MoveTo(State.MovementDestination, Grid, () => {
            CombatEventBus.Moves.Dispatch(State.SelectedUnit, State.MovementDestination);
            State.ResetMovementState();
            animator.SetTrigger("unit_moved");
        });
    }
}