using System.Collections;
using Combat;
using UnityEngine;

public class Moving : StateMachineBehaviour {
    private MapGrid _grid;
    private BattleState _state;
    private BattleManager _battleManager;
    private Animator _animator;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        _state = CombatObjects.GetBattleState();
        _battleManager = CombatObjects.GetBattleManager();
        _grid = CombatObjects.GetMap();
        _animator = animator;

        var unit = _state.SelectedUnit;
        unit.StartCoroutine(DoMove());
    }

    private IEnumerator DoMove() {
        var unit = _state.SelectedUnit;

        var movementHandler = new EventTriggerMovementHandler(_battleManager);
        yield return unit.StartCoroutine(unit.MoveTo(_state.MovementDestination, _grid, movementHandler));

        CombatEventBus.Moves.Dispatch(_state.SelectedUnit, _state.MovementDestination);
        _state.ResetMovementState();
        _animator.SetTrigger("unit_moved");
    }
}