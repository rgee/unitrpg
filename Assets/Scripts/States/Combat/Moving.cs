using Grid;
using UnityEngine;

public class Moving : StateMachineBehaviour {
    private MapGrid Grid;
    private BattleState State;
    private UnitManager UnitManager;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        State = CombatObjects.GetBattleState();
        Grid = CombatObjects.GetMap();
        UnitManager = CombatObjects.GetUnitManager();

        var unit = State.SelectedUnit;
        unit.MoveTo(State.MovementDestination, Grid, arg => {
            var distanceMoved = MathUtils.ManhattanDistance(State.SelectedGridPosition, State.MovementDestination);
            State.MarkUnitMoved(State.SelectedUnit, distanceMoved);
        }, () => {
            CombatEventBus.Moves.Dispatch(State.SelectedUnit, State.MovementDestination);
            State.ResetMovementState();
            animator.SetTrigger("unit_moved");
        });
    }
}