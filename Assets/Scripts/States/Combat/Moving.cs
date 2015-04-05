using UnityEngine;
using System.Collections;

public class Moving : StateMachineBehaviour {
    private BattleState State;
    private MapGrid Grid;
    private Grid.UnitManager UnitManager;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        State = CombatObjects.GetBattleState();
        Grid = CombatObjects.GetMap();
        UnitManager = CombatObjects.GetUnitManager();

        Grid.Unit unit = State.SelectedUnit;
        unit.MoveTo(State.MovementDestination, Grid, (arg) => {
            int distanceMoved = MathUtils.ManhattanDistance(State.SelectedGridPosition, State.MovementDestination);
            State.MarkUnitMoved(State.SelectedUnit, distanceMoved);
        }, () => {
            CombatEventBus.Moves.Dispatch(State.SelectedUnit, State.MovementDestination);
            State.ResetMovementState();
            animator.SetTrigger("unit_moved");
        });
    }
}
