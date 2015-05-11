using Models.Combat;
using UnityEngine;

public class FightEnds : StateMachineBehaviour {
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        var state = CombatObjects.GetBattleState();
        var battle = state.Model;
        Fight fight = new Fight(
            new Participants(
                state.SelectedUnit.model,
                state.AttackTarget.model
            ), 
            AttackType.BASIC,
            new DefaultFightResolution()
        );

        // TODO: Move fight execution into here.
        battle.ExecuteFight(fight);
    }
}