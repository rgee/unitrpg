using Models.Combat;
using UnityEngine;

public class PreviewingFight : CancelableCombatState {
    private Animator Animator;
    private GridCameraController Camera;
    private CombatForecaster Forecaster;
    private FightResult PreviewResult;
    private BattleState State;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        Camera = CombatObjects.GetCameraController();
        Forecaster = CombatObjects.GetCombatForecaster();
        State = CombatObjects.GetBattleState();
        Animator = animator;

        var fight = new Fight(
            new Participants(State.SelectedUnit.GetComponent<Grid.Unit>().model,
                State.AttackTarget.GetComponent<Grid.Unit>().model),
            AttackType.BASIC,
            new DefaultFightResolution()
            );

        PreviewResult = fight.SimulateFight();

        Forecaster.OnConfirm += OnConfirm;
        Forecaster.OnReject += OnReject;
        Forecaster.ShowAttackForecast(PreviewResult, State.SelectedUnit.gameObject, State.AttackTarget.gameObject);

        var attacker = State.SelectedUnit;
        var defender = State.AttackTarget;

        var attackerDirection = MathUtils.DirectionTo(attacker.gridPosition, defender.gridPosition);
        var defenderDirection = attackerDirection.GetOpposite();

        attacker.PrepareForCombat(attackerDirection);
        defender.PrepareForCombat(defenderDirection);

        Camera.DisbleGridSelector();
    }

    private void OnConfirm() {
        State.FightResult = PreviewResult;
        Animator.SetTrigger("attack_confirmed");
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            ReturnUnitsToRest();
        }
        base.OnStateUpdate(animator, stateInfo, layerIndex);
    }

    private void OnReject() {
        ReturnUnitsToRest();
        Animator.SetTrigger("attack_rejected");
    }

    private void ReturnUnitsToRest() {
        State.SelectedUnit.ReturnToRest();
        State.AttackTarget.ReturnToRest();
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        Forecaster.OnConfirm -= OnConfirm;
        Forecaster.OnReject -= OnReject;
        Forecaster.HideCurrentForecast();
        Camera.EnableGridSelector();
    }
}