using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public class DefenderDying : StateMachineBehaviour {

    private Grid.Unit Attacker;
    private Grid.Unit Defender;
    private Animator Animator;
    private GridCameraController Camera;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        Animator = animator;
        Camera = CombatObjects.GetCameraController();

        Camera.Lock();
        BattleState state = CombatObjects.GetBattleState();
        Defender = state.AttackTarget;
        Defender.OnDeath += OnDefenderDeath;
        Attacker = state.SelectedUnit;
    }

    void OnDefenderDeath(object sender, EventArgs e) {
        Attacker.ReturnToRest();
        Animator.SetTrigger("fight_completed");
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        Defender.OnDeath -= OnDefenderDeath;
        Camera.Unlock();
    }
}