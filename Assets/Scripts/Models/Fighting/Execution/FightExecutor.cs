using System;
using System.Collections.Generic;
using UnityEngine;

namespace Models.Fighting.Execution {
    public class FightExecutor {
        public void Execute(FinalizedFight fight) {
            var phases = new List<FightPhase>() {
                fight.InitialPhase,
                fight.FlankerPhase,
                fight.CounterPhase,
                fight.DoubleAttackPhase
            };
            phases.RemoveAll(phase => phase == null);

            phases.ForEach(phase => {
                var receiver = phase.Receiver;
                var effects = phase.Effects.ReceiverEffects;
                var damage = phase.Effects.GetDefenderDamage();
                var remainingHealth = Math.Max(0, receiver.Health - damage);

                if (remainingHealth <= 0) {
                    Debug.LogFormat("{0} should die. Phase says {1}", receiver.Id, phase.ReceverDies);
                }

                effects.ForEach(effect => {
                    effect.Apply(receiver);
                });
            });
        } 
    }
}