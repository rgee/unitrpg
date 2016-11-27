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
                effects.ForEach(effect => {
                    effect.Apply(receiver);
                });
            });
        } 
    }
}