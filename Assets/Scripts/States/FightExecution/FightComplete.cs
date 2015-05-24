using Models.Combat;
using UnityEngine;

public class FightComplete : StateMachineBehaviour {
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        var executor = FightExecutionObjects.GetExecutor();
        if (executor == null) {
            return;
        }

        var state = FightExecutionObjects.GetState();
        if (state.Attacker != null) {
            CommitFightToModels();
            state.Attacker.GetComponent<Grid.Unit>().ReturnToRest();
        }

        if (state.Defender != null) {
            state.Defender.GetComponent<Grid.Unit>().ReturnToRest();
        }
        state.Complete = true;

    }

    private void CommitFightToModels() {
        var battleState = CombatObjects.GetBattleState();
        var battle = battleState.Model;
        Fight fight = new Fight(
            new Participants(
                FightExecutionObjects.GetState().Attacker.GetComponent<Grid.Unit>().model,
                null
            ), 
            AttackType.BASIC,
            new DefaultFightResolution()
        );

        // TODO: Move fight execution into here.
        battle.ExecuteFight(fight);
    }
}